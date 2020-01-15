using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class VNpayController : Controller
    {
        // GET: VNpay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            var order = new Order
            {
                Amount = 10000000,
            };
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", "2.0.0");
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", "ZHIK008H");
            vnPay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString(CultureInfo.InvariantCulture));
            vnPay.AddRequestData("vnp_BankCode", "NCB");
            vnPay.AddRequestData("vnp_BankTranNo", "9704198526191432198");
            vnPay.AddRequestData("vnp_CardType", "ATM");
            vnPay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
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
            return null;
        }
    }
}