using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
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
        public ActionResult Index(string occasion, string type, string color, int? page,
            int? limit, int? minAmount, int? maxAmount, int? sortBy, int? direct, string name)
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
            
            
            

            if (sortBy == null || sortBy > 4 || sortBy < 0)
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            if (direct == null || direct > 1 || direct < 0)
            {
                Debug.WriteLine(direct);
                direct = 0;
            }

            if(name == null)
            {
                name = "";
            }

            var data = db.Products.Where(p =>
                     p.TypeId.Contains(type) &&
                     p.OccasionId.Contains(occasion) &&
                     p.ColorId.Contains(color) &&
                     p.Price >= minAmount &&
                     p.Price <= maxAmount &&
                     p.Name.Contains(name)
                ).ToList();
            var listProduct = new List<Product>();
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (sortBy is (int)FilterEnum.Date)
            {
                if(direct is (int)DirectEnum.Asc)
                {
                    var dataList = data.OrderBy(p => p.CreatedAt);
                    listProduct.AddRange(dataList);
                }
                else
                {
                    var dataList = data.OrderByDescending(p => p.CreatedAt);
                    listProduct.AddRange(dataList);
                }
            }
            else if (sortBy is (int)FilterEnum.Name)
            {
                if(direct is (int)DirectEnum.Asc)
                {
                    var dataList = data.OrderBy(p => p.Name);
                    listProduct.AddRange(dataList);
                }
                else
                {
                    var dataList = data.OrderByDescending(p => p.Name);
                    listProduct.AddRange(dataList);
                }
            }

            else if (sortBy is (int)FilterEnum.Price)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = data.OrderBy(p => p.Price);
                    listProduct.AddRange(dataList);
                }
                else
                {
                    var dataList = data.OrderByDescending(p => p.Price);
                    listProduct.AddRange(dataList);
                }
            }

            else if (sortBy is (int)FilterEnum.SellRate)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = data.OrderBy(p => p.OrderItems.Count);
                    listProduct.AddRange(dataList);
                }
                else
                {
                    var dataList = data.OrderByDescending(p => p.OrderItems.Count);
                    listProduct.AddRange(dataList);
                }
            }            
            else if (sortBy is (int)FilterEnum.InStock)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = data.OrderBy(p => p.InStock);
                    listProduct.AddRange(dataList);
                }
                else
                {
                    var dataList = data.OrderByDescending(p => p.InStock);
                    listProduct.AddRange(dataList);
                }
            }



            ViewBag.sortBy = sortBy;
            Debug.WriteLine(sortBy);
            ViewBag.TotalPage = Math.Ceiling((double)listProduct.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = listProduct.Count;
            ViewBag.Occasion = occasion;
            ViewBag.Type = type;
            ViewBag.Color = color;
            ViewBag.minAmount = minAmount;
            ViewBag.maxAmount = maxAmount;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)DirectEnum.Asc ? (int)DirectEnum.Desc : (int)DirectEnum.Asc;
            ViewBag.name = name;
            listProduct = listProduct.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            foreach (var variable in listProduct)
            {
                Debug.WriteLine("product name: " + variable.Name);
            }

            return View(listProduct);
        }
        public enum FilterEnum
        {
            InStock = 4,           
            SellRate = 3,
            Price = 2,
            Name = 1,
            Date = 0
        }
        public enum DirectEnum
        {
            Asc = 0,
            Desc = 1
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
