using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Flora_Queen_Project.Controllers
{
    [Authorize]
    public class VNpayController : Controller
    {
        // GET: VNpay

        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        protected readonly string CurrentUserId;

        public VNpayController()
        {
            CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public VNpayController(
            ApplicationUserManager userManager
        )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout()
        {
            Debug.WriteLine(CurrentUserId);
            var order = new Order
            {
                ApplicationUserId = CurrentUserId,
                Amount = 1000000,
            };
            _db.ApplicationOrders.Add(order);
            _db.SaveChanges();
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", "2.0.0");
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", "ZHIK008H");
            vnPay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString(CultureInfo.InvariantCulture));
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

        public ActionResult PaymentResult(PaymentResultViewModel paymentResult)
        {
            if (paymentResult == null)
            {
                return HttpNotFound();
            }
            return View(paymentResult);
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
    }
}