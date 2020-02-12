using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace Flora_Queen_Project.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        protected readonly string CurrentUserId;


        public PaymentController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        public PaymentController(
            ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        
        public async Task<ActionResult> Checkout()
        {
            if (!(Session["ShoppingCart"] is List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
                Debug.WriteLine("list null");
            }
            Session["ShoppingCart"] = shoppingCart;
            if (!shoppingCart.Any())
            {
                TempData["Display"] = "show";
                return RedirectToAction("Index", "Shop");
            }

            ViewBag.ShopCartReview = shoppingCart;
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var model = new CheckoutViewModel
            {
                Address = user.Address,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Lastname = user.LastName,
                Firstname = user.FirstName,
                ZipCode = user.Zipcode,
                CompanyName = user.CompanyName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            var getPriceVnd = new HttpClient();
            double usdVnd;
            try
            {
                var responseContent = getPriceVnd.GetAsync("https://free.currconv.com/api/v7/convert?q=USD_VND&compact=ultra&apiKey=549fdf0ffb9b928a00a6").Result.Content.ReadAsStringAsync().Result;
                var exchangeRate = JsonConvert.DeserializeObject<ObservableCollection<CurrencyConvertModel>>(responseContent);
                var first = exchangeRate?.FirstOrDefault();

                usdVnd = first?.USD_VND ?? 23238.5;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                usdVnd = 23238.5;
            }

            if (!(Session["ShoppingCart"] is List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
                Debug.WriteLine("list null");
            }
            Session["ShoppingCart"] = shoppingCart;
            if (!shoppingCart.Any())
            {
                TempData["Display"] = "show";
                return RedirectToAction("Index", "Shop");
            }
            if (!ModelState.IsValid) return View(model);
            var total = Math.Round(shoppingCart.Sum(item => item.total), 2);

            var order = new Order
            {
                Amount = total,
                ApplicationUserId = CurrentUserId,
                ShipName = model.Firstname + model.Lastname,
                ShipAddress = model.Address,
                ShipEmail = model.Email,
                ShipPhone = model.Phone,
                PaymentMethod = model.PaymentMethod
            };

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var orderItems = new List<OrderItem>();
                    foreach (var item in shoppingCart)
                    {
                        var product = await _db.Products.FindAsync(item.id);
                        if (product == null)
                        {
                            return HttpNotFound();
                        }

                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = item.count,
                            Discount = item.discount,
                            UnitPrice = item.price
                        };
                        orderItems.Add(orderItem);
                        product.InStock -= orderItem.Quantity;
                        if (product.InStock < 0)
                        {
                            var paymentResult = new PaymentResultViewModel();
                            ViewBag.Response = "Fail";
                            return RedirectToAction("PaymentResult", paymentResult);
                        }
                       
                        _db.Entry(product);
                    }

                    order.OrderItems = orderItems;
                    _db.ApplicationOrders.Add(order);
                    await _db.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Debug.WriteLine(e);
                }
            }
            

            Session.Remove("ShoppingCart");
            Session["orderId"] = order.Id;

            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", "2.0.0");
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", "ZHIK008H");
            vnPay.AddRequestData("vnp_Amount", ((int) order.Amount * usdVnd).ToString(CultureInfo.InvariantCulture));
            vnPay.AddRequestData("vnp_BankCode", "NCB");
            vnPay.AddRequestData("vnp_BankTranNo", "9704198526191432198");
            vnPay.AddRequestData("vnp_CardType", "ATM");
            vnPay.AddRequestData("vnp_CreateDate", order.CreatedAt.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Order test");
            vnPay.AddRequestData("vnp_OrderType", "other");
            if (Request.Url != null)
                vnPay.AddRequestData("vnp_ReturnUrl", Url.Action("Ipn", "VNpay", null, Request.Url.Scheme));
            vnPay.AddRequestData("vnp_TxnRef", order.Id);
            var paymentUrl = vnPay.CreateRequestUrl("http://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "EAXOMHDGJEPMFEMDIXJZHHPPXEXMSGFD");
            return Redirect(paymentUrl);
        }

        public ActionResult Ipn()
        {
            string returnContent;
            var paymentResult = new PaymentResultViewModel();
            if (Request.QueryString.Count > 0)
            {
                const string vnpHashSecret = "EAXOMHDGJEPMFEMDIXJZHHPPXEXMSGFD"; //Secret key
                var vnPayData = Request.QueryString;
                var vnPay = new VnPayLibrary();


                foreach (string s in vnPayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnPay.AddResponseData(s, vnPayData[s]);
                    }
                }


                var orderId = vnPay.GetResponseData("vnp_TxnRef");
                // ReSharper disable once UnusedVariable
                var vnPayTranId = vnPay.GetResponseData("vnp_TransactionNo");
                var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
                var vnpSecureHash = Request.QueryString["vnp_SecureHash"];
                var checkSignature = vnPay.ValidateSignature(vnpSecureHash, vnpHashSecret);

                if (checkSignature)
                {

                    var order = _db.ApplicationOrders.Find(orderId);
                    if (order != null)
                    {
                        if (order.OrderStatus == Order.OrderStatusEnum.Pending)
                        {
                            if (vnpResponseCode == "00")
                            {
                                order.OrderStatus = Order.OrderStatusEnum.Paid;
                                paymentResult.PaymentStatus = PaymentResultViewModel.PaymentStatusEnum.Success;
                                paymentResult.Amount = order.Amount;
                            }
                            else
                            {
                                order.OrderStatus = Order.OrderStatusEnum.Cancel;
                                paymentResult.PaymentStatus = PaymentResultViewModel.PaymentStatusEnum.Fail;
                                paymentResult.Amount = order.Amount;
                            }


                            _db.SaveChanges();
                            returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                        }
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                    }
                }
                else
                {
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            else
            {
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"Input data required\"}";
            }

            ViewBag.Response = returnContent;
            Response.ClearContent();
            return RedirectToAction("PaymentResult", paymentResult);
        }

        public ActionResult PaymentResult(PaymentResultViewModel paymentResult)
        {
            if (paymentResult == null)
            {
                return HttpNotFound();
            }
            return View(paymentResult);
        }

        public class CurrencyConvertModel
        {
            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once InconsistentNaming
            public double USD_VND { get; set; }
        }
    }
}