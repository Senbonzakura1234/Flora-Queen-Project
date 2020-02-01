using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(string occasion, string type, string color, int? minAmount, int? maxAmount, int? viewMode, int? sortBy)
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

            if (viewMode == null || viewMode > 1 || viewMode < 0)
            {
                viewMode = 0;
            }

            if (sortBy == null || sortBy > 5 || sortBy < 0)
            {
                sortBy = 0;
            }
            var data = db.Products.Where(p =>
            p.TypeId.Contains(type) &&
            p.OccasionId.Contains(occasion) &&
            p.ColorId.Contains(color) &&
            p.Price >= minAmount &&
            p.Price <= maxAmount
            ).ToList();
            var listProduct = new List<Product>();

            if (sortBy is (int)FilterEnum.Date)
            {
                var dataList = data.OrderByDescending(p => p.CreatedAt);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int)FilterEnum.NameAsc)
            {
                var dataList = data.OrderBy(p => p.Name);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int)FilterEnum.NameDesc)
            {
                var dataList = data.OrderByDescending(p => p.Name);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int)FilterEnum.PriceAsc)
            {
                var dataList = data.OrderBy(p => p.Price);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int)FilterEnum.PriceDesc)
            {
                var dataList = data.OrderByDescending(p => p.Price);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int)FilterEnum.SellRate)
            {
                var dataList = data.OrderByDescending(p => p.OrderItems.Count);
                listProduct.AddRange(dataList);
            }
            else
            {
                var dataList = data.OrderByDescending(p => p.CreatedAt);
                listProduct.AddRange(dataList);
            }

            ViewBag.sortBy = sortBy;

            ViewBag.TotalItem = listProduct.Count;
            ViewBag.Occasion = occasion;
            ViewBag.Type = type;
            ViewBag.Color = color;
            ViewBag.minAmount = minAmount;
            ViewBag.maxAmount = maxAmount;
            ViewBag.viewMode = viewMode;

            listProduct = listProduct.ToList();

            return View(listProduct);
        }
        public enum FilterEnum
        {
            [Display(Name = "Sell rate")]
            SellRate = 5,
            [Display(Name = "Price Descending")]
            PriceDesc = 4,
            [Display(Name = "Price Ascending")]
            PriceAsc = 3,
            [Display(Name = "Name Descending")]
            NameDesc = 2,
            [Display(Name = "Name Ascending")]
            NameAsc = 1,
            [Display(Name = "Date")]
            Date = 0
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name");
            ViewBag.OccasionId = new SelectList(db.Occasions, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ImgUrl,InStock,Price,TypeId,OccasionId,ColorId,ProductStatus,CreatedAt,UpdatedAt,DeletedAt")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ImgUrl,InStock,Price,TypeId,OccasionId,ColorId,ProductStatus,CreatedAt,UpdatedAt,DeletedAt")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var product = db.Products.Find(id);
            product.DeletedAt = DateTime.Now;
            product.ProductStatus = 0;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
