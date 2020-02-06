using System;
using System.Collections.Generic;
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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(string occasion, string type, string color, int? page,
            int? limit, double? minPrice, double? maxPrice, int? sortBy, int? direct, string name, int? month, int? year)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 9;
            }


            var data = _db.Products.Where(p => p.ProductStatus != Product.ProductStatusEnum.Removed).ToList();
            var totalItem = data.Count;
            if (name == null)
            {
                name = "";
            }
            data = data.Where(p => p.Name.Contains(name)).ToList();

            if (!string.IsNullOrWhiteSpace(occasion))
            {
                data = data.Where(p => p.OccasionId == occasion).ToList();
            }
            else
            {
                occasion = "";
            }


            if (!string.IsNullOrWhiteSpace(type))
            {
                data = data.Where(p => p.TypeId == type).ToList();
            }
            else
            {
                type = "";
            }


            if (!string.IsNullOrWhiteSpace(color))
            {
                data = data.Where(p => p.ColorId == color).ToList();
            }
            else
            {
                color = "";
            }


            
            if (minPrice == null)
            {
                minPrice = 0;
            }
            if (maxPrice == null)
            {
                maxPrice = 100;
            }            
            data = data.Where(p => p.Price <= maxPrice && p.Price >= minPrice).ToList();

            if (month != null && month >= 1 && month <= 12 && year != null)
            {
                data = data.Where(p => p.CreatedAt.Month == month && p.CreatedAt.Year == year).ToList();
            }
            else
            {
                month = 0;
                year = 0;
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

            
            var listProduct = new List<Product>();
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (sortBy is (int)SortEnum.Date)
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
            else if (sortBy is (int)SortEnum.Name)
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

            else if (sortBy is (int)SortEnum.Price)
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

            else if (sortBy is (int)SortEnum.SellRate)
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
            else if (sortBy is (int)SortEnum.InStock)
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
            else
            {
                var dataList = data.OrderBy(p => p.CreatedAt);
                listProduct.AddRange(dataList);
            }



            ViewBag.sortBy = sortBy;
            Debug.WriteLine(sortBy);
            ViewBag.TotalPage = Math.Ceiling((double)listProduct.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = totalItem;

            ViewBag.occasion = occasion;
            ViewBag.type = type;
            ViewBag.color = color;

            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;

            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)DirectEnum.Asc ? (int)DirectEnum.Desc : (int)DirectEnum.Asc;
            ViewBag.name = name;
            ViewBag.month = month;
            ViewBag.year = year;
            listProduct = listProduct.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            foreach (var variable in listProduct)
            {
                Debug.WriteLine("product name: " + variable.Name);
            }

            return View(listProduct);
        }
        public enum SortEnum
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
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ColorId = new SelectList(_db.Colors, "Id", "Name");
            ViewBag.OccasionId = new SelectList(_db.Occasions, "Id", "Name");
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Name");
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
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ColorId = new SelectList(_db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(_db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.ColorId = new SelectList(_db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(_db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Name", product.TypeId);
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
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ColorId = new SelectList(_db.Colors, "Id", "Name", product.ColorId);
            ViewBag.OccasionId = new SelectList(_db.Occasions, "Id", "Name", product.OccasionId);
            ViewBag.TypeId = new SelectList(_db.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
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
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            product.DeletedAt = DateTime.Now;
            product.ProductStatus = Product.ProductStatusEnum.Removed;
            _db.Entry(product).State = EntityState.Modified;

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
