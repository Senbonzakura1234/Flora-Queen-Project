using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
            }

            order.OrderItems = orderItems;
            _db.ApplicationOrders.Add(order);
            await _db.SaveChangesAsync();

            Session.Remove("ShoppingCart");
            return RedirectToAction("Index", "Home");
        }
    }
}