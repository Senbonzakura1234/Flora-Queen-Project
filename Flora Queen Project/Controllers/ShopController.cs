using System;
using System.Collections.Generic;
using System.Diagnostics;
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
//        public ActionResult Index(int? page, int? limit)
//        {
//            if (page == null)
//            {
//                page = 1;
//            }
//
//            if (limit == null)
//            {
//                limit = 9;
//            }
//
//            var listProduct = _db.Products.OrderByDescending(p => p.UpdatedAt).ToList();
//            Debug.WriteLine("count: " + listProduct.Count);
//            ViewBag.TotalPage = Math.Ceiling((double)listProduct.Count() / limit.Value);
//            ViewBag.CurrentPage = page;
//            ViewBag.Limit = limit;
//
//            listProduct = listProduct.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
//
//            return View(listProduct);
//        }

        public ActionResult Index(string occasion, string type, string color, int? page, int? limit, int? minAmount, int? maxAmount)
        {
            if (occasion == null)
            {
                occasion = "";
            }

            if (type == null)
            {
                type = "";
            }

            if (color == null)
            {
                color = "";
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 9;
            }

            if (minAmount == null)
            {
                minAmount = 0;
            }

            if (maxAmount == null)
            {
                maxAmount = 500;
            }

            var listProduct = _db.Products.OrderByDescending(p => p.UpdatedAt).Where(p =>
                p.TypeId.Contains(type) &&
                p.OccasionId.Contains(occasion) && 
                p.ColorId.Contains(color) && 
                p.Price >= minAmount*1000 &&
                p.Price <= maxAmount*1000
                ).ToList();

            ViewBag.TotalPage = Math.Ceiling((double)listProduct.Count() / limit.Value);
            ViewBag.CurrentPage = page;
            ViewBag.Limit = limit;

            ViewBag.Occasion = occasion;
            ViewBag.Type = type;
            ViewBag.Color = color;
            ViewBag.minAmount = minAmount;
            ViewBag.maxAmount = maxAmount;

            listProduct = listProduct.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            foreach (var variable in listProduct)
            {
                Debug.WriteLine("product name: " + variable.Name);
            }

            return View(listProduct);
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

            var relateData = DbContext.Products.Where(p => p.OccasionId == product.OccasionId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList();
            relateData.AddRange(DbContext.Products.Where(p => p.ColorId == product.ColorId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList());
            relateData.AddRange(DbContext.Products.Where(p => p.TypeId == product.TypeId && p.Id != product.Id).OrderByDescending(p => p.UpdatedAt).Take(2).ToList());
            ViewBag.relateData = relateData;

            var saleData = DbContext.Products.OrderByDescending(p => p.UpdatedAt).Take(6).ToList();
            ViewBag.saleData = saleData;

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