using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
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
        private ApplicationDbContext _db;

        public VNpayController()
        {
        }

        public VNpayController(
            ApplicationUserManager userManager,
            ApplicationDbContext dbContext
        )
        {
            DbContext = dbContext;
            UserManager = userManager;
        }

        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
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
        public ActionResult Checkout(double amount)
        {
            var userId = User.Identity.GetUserId();
            if (amount <= 0)
            {
                amount = 1000000;
            }
            var order = new Order
            {
                ApplicationUserId = userId,
                Amount = amount,
            };
            DbContext.ApplicationOrders.Add(order);
            DbContext.SaveChanges();
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

        public ActionResult PaymentResult()
        {
            return View();
        }
        public ActionResult Ipn()
        {
            string returnContent;
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


                var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef"));
                // ReSharper disable once UnusedVariable
                var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
                var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
                var vnpSecureHash = Request.QueryString["vnp_SecureHash"];
                var checkSignature = vnPay.ValidateSignature(vnpSecureHash, vnpHashSecret);

                if (checkSignature)
                {

                    var order = DbContext.ApplicationOrders.Find(orderId);
                    if (order != null)
                    {
                        if (order.OrderStatus == Order.OrderStatusEnum.Pending)
                        {
                            order.OrderStatus = vnpResponseCode == "00" ? Order.OrderStatusEnum.Paid : Order.OrderStatusEnum.Cancel;


                            DbContext.SaveChanges();
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
            Response.Write(returnContent);
            Response.End();
            return Redirect("PaymentResult");
        }
    }
}