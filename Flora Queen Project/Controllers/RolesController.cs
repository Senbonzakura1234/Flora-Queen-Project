using System.Threading.Tasks;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Flora_Queen_Project.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly RoleManager<ApplicationUserRole> _roleManager;

        public RolesController()
        {
            var roleStore = new RoleStore<ApplicationUserRole>(_db);
            _roleManager = new RoleManager<ApplicationUserRole>(roleStore);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddRole(ApplicationUserRole role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }
    }
}