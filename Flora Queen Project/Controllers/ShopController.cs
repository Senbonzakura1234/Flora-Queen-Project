﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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


        public ActionResult Index(string occasion, string type, string color, int? page, int? limit, int? minAmount, int? maxAmount, int? viewMode, int? sortBy)
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

            if (viewMode == null || viewMode > 1 || viewMode < 0)
            {
                viewMode = 0;
            }
            
            if (sortBy == null || sortBy > 5 || sortBy < 0)
            {
                Debug.WriteLine(sortBy);
                sortBy = 0;
            }

            var data = _db.Products.Where(p =>
                p.TypeId.Contains(type) &&
                p.OccasionId.Contains(occasion) &&
                p.ColorId.Contains(color) &&
                p.Price >= minAmount &&
                p.Price <= maxAmount
            ).ToList();
            var listProduct = new List<Product>();
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (sortBy is (int) SortEnum.Date)
            {
                var dataList = data.OrderByDescending(p => p.CreatedAt);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int) SortEnum.NameAsc)
            {
                var dataList = data.OrderBy(p => p.Name);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int) SortEnum.NameDesc)
            {
                var dataList = data.OrderByDescending(p => p.Name);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int) SortEnum.PriceAsc)
            {
                var dataList = data.OrderBy(p => p.Price);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int) SortEnum.PriceDesc)
            {
                var dataList = data.OrderByDescending(p => p.Price);
                listProduct.AddRange(dataList);
            }
            else if (sortBy is (int) SortEnum.SellRate)
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
            ViewBag.viewMode = viewMode;

            listProduct = listProduct.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();

            foreach (var variable in listProduct)
            {
                Debug.WriteLine("product name: " + variable.Name);
            }

            return View(listProduct);
        }
        public enum SortEnum
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

        //Shopping Cart
        public async Task<ActionResult> UpdateCart(string proId, int quantity, string dataAction)
        {
            if (!(Session["ShoppingCart"] is List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
                Debug.WriteLine("list null");
            }

            if (!string.IsNullOrEmpty(dataAction))
            {
                if (dataAction != "delete")
                {
                    var productExist = true;
                    for (var i = 0; i < shoppingCart.Count; i++)
                    {
                        if (shoppingCart[i].id != proId) continue;
                        if (dataAction != "minusOne")
                        {
                            shoppingCart[i].count += quantity;
                            shoppingCart[i].total =
                                Math.Round(shoppingCart[i].price * shoppingCart[i].discount * shoppingCart[i].count, 2);
                        }
                        else
                        {
                            shoppingCart[i].count -= 1;
                            shoppingCart[i].total =
                                Math.Round(shoppingCart[i].price * shoppingCart[i].discount * shoppingCart[i].count, 2);
                        }

                        if (shoppingCart[i].count <= 0)
                        {
                            shoppingCart.RemoveAt(i);
                        }

                        productExist = false;
                        break;
                    }

                    if (productExist)
                    {
                        var product = await DbContext.Products.FindAsync(proId);
                        if (product == null) return Json(new { shoppingCart }, JsonRequestBehavior.AllowGet);

                        shoppingCart.Add(new CartItem
                        {
                            id = product.Id,
                            name = product.Name,
                            imgUrl = product.ImgUrl,
                            count = quantity,
                            discount = product.Discount,
                            price = product.Price,
                            total = Math.Round(product.Price * product.Discount * quantity, 2)
                        });
                    }
                }
                else
                {
                    for (var i = 0; i < shoppingCart.Count; i++)
                    {
                        if (shoppingCart[i].id != proId) continue;
                        shoppingCart.RemoveAt(i);
                        break;
                    }
                }
            }

            Session["ShoppingCart"] = shoppingCart;

            return Json(new { shoppingCart }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ShowCartAjax()
        {
            if (!(Session["ShoppingCart"] is List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
                Debug.WriteLine("list null");
            }

            Session["ShoppingCart"] = shoppingCart;
            return PartialView(shoppingCart);
        }

        public ActionResult Cart()
        {
            if (!(Session["ShoppingCart"] is List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
                Debug.WriteLine("list null");
            }

            Session["ShoppingCart"] = shoppingCart;
            return View(shoppingCart);
        }
    }
}