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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(double? minAmount, double? maxAmount, int? status, int? month, int? year, int? page,
            int? limit, int? sortBy, int? direct, int? payment, string shipName )
        {
            var orders = _db.ApplicationOrders.Where(o => o.OrderStatus != Order.OrderStatusEnum.Deleted).ToList();

            if (string.IsNullOrWhiteSpace(shipName))
            {
                shipName = "";
            }
            orders = orders.Where(o => o.ShipName.Contains(shipName)).ToList();

            if (minAmount == null || minAmount < 0)
            {
                minAmount = 0;
            }
            orders = orders.Where(o => o.Amount >= minAmount).ToList();

            if (maxAmount == null || maxAmount < 0)
            {
                maxAmount = 500;
            }
            orders = orders.Where(o => o.Amount <= maxAmount).ToList();

            if (status != null && Enum.IsDefined(typeof(Order.OrderStatusEnum), status))
            {
                orders = orders.Where(o => o.OrderStatus == (Order.OrderStatusEnum) status).ToList();
            }
            else
            {
                status = 5;
            }

            if (payment != null && Enum.IsDefined(typeof(Order.OrderPaymentMethodEnum), payment))
            {
                orders = orders.Where(o => o.PaymentMethod == (Order.OrderPaymentMethodEnum) payment).ToList();
            }
            else
            {
                payment = 4;
            }

            if (month != null && month.Value >= 1 && month.Value <= 12 && year != null)
            {
                orders = orders.Where(o => o.CreatedAt.Month == month && o.CreatedAt.Year == year).ToList(); 
            }
            else
            {
                month = 0;
                year = 0;
            }

            if (sortBy == null || !Enum.IsDefined(typeof(SortEnum), sortBy))
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }
            if (direct == null || !Enum.IsDefined(typeof(DirectEnum), direct))
            {
                Debug.WriteLine(direct);
                direct = 0;
            }

            var listOrder = new List<Order>();

            if (sortBy is (int) SortEnum.Date)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = orders.OrderBy(p => p.CreatedAt);
                    listOrder.AddRange(dataList);
                }
                else
                {
                    var dataList = orders.OrderByDescending(p => p.CreatedAt);
                    listOrder.AddRange(dataList);
                }
            }
            else if (sortBy is (int) SortEnum.Name)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = orders.OrderBy(p => p.ShipName);
                    listOrder.AddRange(dataList);
                }
                else
                {
                    var dataList = orders.OrderByDescending(p => p.ShipName);
                    listOrder.AddRange(dataList);
                }
            }
            else if (sortBy is (int) SortEnum.Status)
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = orders.OrderBy(p => p.OrderStatus);
                    listOrder.AddRange(dataList);
                }
                else
                {
                    var dataList = orders.OrderByDescending(p => p.OrderStatus);
                    listOrder.AddRange(dataList);
                }
            }
            else if (sortBy is (int) SortEnum.Amount)
            {
                if (direct is (int) DirectEnum.Asc)
                {
                    var dataList = orders.OrderBy(p => p.Amount);
                    listOrder.AddRange(dataList);
                }
                else
                {
                    var dataList = orders.OrderByDescending(p => p.Amount);
                    listOrder.AddRange(dataList);
                }
            }
            else
            {
                if (direct is (int)DirectEnum.Asc)
                {
                    var dataList = orders.OrderBy(p => p.PaymentMethod);
                    listOrder.AddRange(dataList);
                }
                else
                {
                    var dataList = orders.OrderByDescending(p => p.PaymentMethod);
                    listOrder.AddRange(dataList);
                }
            }

            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }

            ViewBag.shipName = shipName;
            ViewBag.sortBy = sortBy;
            ViewBag.direct = direct;
            ViewBag.directSet = direct is (int)DirectEnum.Asc ? (int)DirectEnum.Desc : (int)DirectEnum.Asc;
            Debug.WriteLine(sortBy);
            ViewBag.TotalPage = Math.Ceiling((double)listOrder.Count / limit.Value);
            ViewBag.CurrentPage = page;

            ViewBag.Limit = limit;

            ViewBag.TotalItem = listOrder.Count;
           
            ViewBag.minAmount = minAmount;
            ViewBag.maxAmount = maxAmount;
            ViewBag.status = status;
            ViewBag.payment = payment;
            ViewBag.month = month;
            ViewBag.year = year;

            listOrder = listOrder.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            return View(listOrder.ToList());
        }

        public enum SortEnum
        {
            PaymentMethod = 4,
            Status = 3,
            Amount = 2,
            Name = 1,
            Date = 0
        }
        public enum DirectEnum
        {
            Asc = 0,
            Desc = 1
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _db.ApplicationOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.listOrderItem = _db.OrderItems.Where(od => od.OrderId == id).ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ReSharper disable once InconsistentNaming
        public ActionResult Edit(string id, int OrderStatus)
        {
            var order = _db.ApplicationOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.OrderStatus = (Order.OrderStatusEnum) OrderStatus;

            if (ModelState.IsValid)
            {
                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
         
            return RedirectToAction("Details", "Orders", id);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = _db.ApplicationOrders.Find(id);
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
            var order = _db.ApplicationOrders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.OrderStatus = Order.OrderStatusEnum.Deleted;
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
