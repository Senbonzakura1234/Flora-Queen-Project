using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext _db;

        public ShopController()
        {

        }
        public ShopController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Single() //string id
        {
            return View();
        }

        public ActionResult OccasionsSideBar()
        {
            var data = DbContext.Occasions.ToList();
            return PartialView(data);
        }

        public ActionResult TypesSideBar()
        {
            var data = DbContext.Types.ToList();
            return PartialView(data);
        }

        public ActionResult ColorsSideBar()
        {
            var data = DbContext.Colors.ToList();
            return PartialView(data);
        }

        public ActionResult HotDeal()
        {
            var data = DbContext.Products.OrderBy(p => p.Name).FirstOrDefault();
            return PartialView(data);
        }
    }
}