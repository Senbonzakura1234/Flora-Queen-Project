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
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(double? minPrice, double? maxPrice, int? status, int? month, int? year, int? page, int? limit, int? sortBy)
        {
            var orders = db.ApplicationOrders.ToList();
            if (minPrice != null && minPrice.Value > 0)
            {
                orders = orders.Where(o => o.Amount >= minPrice).ToList();

                if (maxPrice != null && maxPrice.Value >= minPrice)
                {
                    orders = orders.Where(o => o.Amount <= maxPrice).ToList();
                }
            }

            if (status != null)
            {
                orders = orders.Where(o => o.OrderStatus == (Order.OrderStatusEnum) status.Value).ToList();
            }

            if (month != null && month.Value >= 1 && month.Value <= 12 && year != null)
            {
                orders = orders.Where(o => o.CreatedAt.Month == month && o.CreatedAt.Year == year).ToList();
            }

            if (sortBy == null || sortBy > 4 || sortBy < 0)
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            var listOrder = new List<Order>();
            if (sortBy is (int)SortEnum.Date)
            {
                var dataList = orders.OrderByDescending(p => p.CreatedAt);
                listOrder.AddRange(dataList);
            }
            else if (sortBy is (int)SortEnum.Name)
            {
                var dataList = orders.OrderBy(p => p.ApplicationUser.UserName);
                listOrder.AddRange(dataList);
            }
            else if (sortBy is (int)SortEnum.Status)
            {
                var dataList = orders.OrderBy(p => p.OrderStatus);
                listOrder.AddRange(dataList);
            }
            else if (sortBy is (int)SortEnum.AmountAsc)
            {
                var dataList = orders.OrderBy(p => p.Amount);
                listOrder.AddRange(dataList);
            }
            else
            {
                var dataList = orders.OrderByDescending(p => p.Amount);
                listOrder.AddRange(dataList);
            }

            if (page != null)
            {
                page = 1;
            }

            if (limit != null)
            {
                limit = 10;
            }

            ViewBag.sortBy = sortBy;
            Debug.WriteLine(sortBy);
            ViewBag.TotalPage = Math.Ceiling((double)listOrder.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = listOrder.Count;
           
            ViewBag.minAmount = minPrice;
            ViewBag.maxAmount = maxPrice;
            ViewBag.status = status;

            listOrder = listOrder.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            //ViewBag.orderStatus = 
            return View(listOrder.ToList());
        }

        public enum SortEnum
        {
            [Display(Name = "Price Ascending")]
            Status = 4,
            [Display(Name = "Price Ascending")]
            AmountDesc = 3,
            [Display(Name = "Name Descending")]
            AmountAsc = 2,
            [Display(Name = "Name Ascending")]
            Name = 1,
            [Display(Name = "Date")]
            Date = 0
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.ApplicationOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.listOrderItem = db.OrderItems.Where(od => od.OrderId == id).ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, int OrderStatus)
        {
            var order = db.ApplicationOrders.Find(id);
            order.OrderStatus = (Order.OrderStatusEnum) OrderStatus;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
         
            return View("Details/" + id);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.ApplicationOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var order = db.ApplicationOrders.Find(id);
            db.ApplicationOrders.Remove(order);
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
