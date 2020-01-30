using Flora_Queen_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flora_Queen_Project.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        private ApplicationDbContext _db;

        public ChartController()
        {

        }
        public ChartController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }
        public JsonResult GetColorData()
        {
            var data = DbContext.Colors.ToList();
            var listColor = new List<ColorsJsonModel>();
            foreach (var item in data) {
                listColor.Add(new ColorsJsonModel { name = item.Name, count = item.Products.Count()});
            }
            return Json(new
            {
                listColor
            }, JsonRequestBehavior.AllowGet);
        }        
        public JsonResult GetOccasionData()
        {
            var data = DbContext.Occasions.ToList();
            var listOccasion = new List<OccasionsJsonModel>();
            foreach (var item in data) {
                listOccasion.Add(new OccasionsJsonModel { name = item.Name, count = item.Products.Count()});
            }
            return Json(new
            {
                listOccasion
            }, JsonRequestBehavior.AllowGet);
        }        
        public JsonResult GetTypeData()
        {
            var data = DbContext.Types.ToList();
            var listType = new List<TypesJsonModel>();
            foreach (var item in data) {
                listType.Add(new TypesJsonModel { name = item.Name, count = item.Products.Count()});
            }
            return Json(new
            {
                listType
            }, JsonRequestBehavior.AllowGet);
        }

    }
}