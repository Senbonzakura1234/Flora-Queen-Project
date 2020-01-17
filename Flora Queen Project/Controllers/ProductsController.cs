using System;
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
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Color).Include(p => p.Occasion).Include(p => p.Type);
            return View(products.ToList());
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
