using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

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

        public ActionResult Single(string id) //string id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var listdata = DbContext.Products.Where(p => p.OccasionId == product.OccasionId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList();
            listdata.AddRange(DbContext.Products.Where(p => p.ColorId == product.ColorId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList());
            listdata.AddRange(DbContext.Products.Where(p => p.TypeId == product.TypeId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList());
            ViewBag.listdata = listdata;

            var listSale = DbContext.Products.OrderByDescending(p => p.UpdatedAt).Take(6).ToList();
            ViewBag.listSale = listSale;

            return View(product);
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