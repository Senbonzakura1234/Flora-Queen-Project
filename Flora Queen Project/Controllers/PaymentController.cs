using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Flora_Queen_Project.Controllers
{
    public class PaymentController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;

        public PaymentController()
        {

        }
        public PaymentController(
            ApplicationDbContext dbContext,
            ApplicationUserManager userManager)
        {
            UserManager = userManager;
            DbContext = dbContext;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }
        // GET: Payment
        public ActionResult Cart()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View();
        }
    }
}