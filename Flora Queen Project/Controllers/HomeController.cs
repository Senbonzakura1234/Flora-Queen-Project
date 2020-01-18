using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Flora_Queen_Project.Models;

namespace Flora_Queen_Project.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController()
        {

        }
        public HomeController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }
        public ActionResult Index()
        {
            var occasions = DbContext.Occasions.ToList();
            ViewBag.OccasionList = occasions;
            var data = new List<Product>();
            foreach (var item in occasions)
            {
                data.AddRange(DbContext.Products.Where(p => p.OccasionId == item.Id).Take(5));
            }
            //var data = DbContext.Products.ToList();
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // ReSharper disable once InconsistentNaming
        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult NewProduct()
        {
            var data = DbContext.Products.OrderByDescending(t => t.CreatedAt).Take(5).ToList();
            return PartialView(data);
        }

        public ActionResult BestSales()
        {
            var data = DbContext.Products.OrderBy(t => t.CreatedAt).Take(8).ToList();
            return PartialView(data);
        }

        public ActionResult OccasionMenu()
        {
            var data = DbContext.Occasions.ToList();
            return PartialView(data);
        }

        public ActionResult TypeMenu()
        {
            var data = DbContext.Types.ToList();
            return PartialView(data);
        }

        public ActionResult ColorMenu()
        {
            var data = DbContext.Colors.ToList();
            return PartialView(data);
        }

        public async Task<ActionResult> QuickView(string id)
        {
            Debug.WriteLine(id);
            if (id == null)
            {
                return null;
            }
            var product = await DbContext.Products.FindAsync(id);
            return product == null ? null : PartialView("_AjaxQuickView", product);
        }
    }
}