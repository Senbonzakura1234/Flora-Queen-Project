using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class OccasionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Occasions
        public ActionResult Index()
        {
            return View(db.Occasions.ToList());
        }

        // GET: Occasions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occasion occasion = db.Occasions.Find(id);
            if (occasion == null)
            {
                return HttpNotFound();
            }
            return View(occasion);
        }

        // GET: Occasions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Occasions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt,UpdatedAt,DeletedAt,OccasionStatus")] Occasion occasion)
        {
            if (ModelState.IsValid)
            {
                db.Occasions.Add(occasion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(occasion);
        }

        // GET: Occasions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occasion occasion = db.Occasions.Find(id);
            if (occasion == null)
            {
                return HttpNotFound();
            }
            return View(occasion);
        }

        // POST: Occasions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedAt,UpdatedAt,DeletedAt,OccasionStatus")] Occasion occasion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(occasion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(occasion);
        }

        // GET: Occasions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occasion occasion = db.Occasions.Find(id);
            if (occasion == null)
            {
                return HttpNotFound();
            }
            return View(occasion);
        }

        // POST: Occasions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Occasion occasion = db.Occasions.Find(id);
            occasion.DeletedAt = DateTime.Now;
            occasion.OccasionStatus = 0;
            db.Entry(occasion).State = EntityState.Modified;
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
