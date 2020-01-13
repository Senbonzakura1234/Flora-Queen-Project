using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Flora_Queen_Project.Controllers
{
    [Authorize(Roles = "Admin,Mod")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _db;
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationUserManager _userManager;

        public AdminController()
        {

        }
        public AdminController(
            ApplicationDbContext dbContext,
            ApplicationUserManager userManager,
            RoleManager<ApplicationUserRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            DbContext = dbContext;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }

        public RoleManager<ApplicationUserRole> RoleManager
        {
            get => _roleManager ?? new RoleManager<ApplicationUserRole>(new RoleStore<ApplicationUserRole>(_db));
            private set => _roleManager = value;
        }

        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var users = await DbContext.Users.ToListAsync();
            var data = users.Select(
                user => new AccountManageViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = DbContext.Roles.Find(user.Roles.FirstOrDefault()?.RoleId).Name
                }).OrderBy(p => p.Role).ToList();
            return View(data);
        }
        
        public ActionResult Details(string id)
        {
            if(string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = DbContext.Users.Find(id);
            if(user == null)
            {
                return HttpNotFound();
            }

            var data = new AccountManageViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = DbContext.Roles.Find(user.Roles.FirstOrDefault()?.RoleId).Name,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                UserStatus = user.UserStatus,
                Birthday = user.Birthday,
                Avatar = user.Avatar,
                Description = user.Description
            };
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = DbContext.Users.Find(id);
            if (user == null) return HttpNotFound();
            var oldRole = DbContext.Roles.Find(user.Roles.FirstOrDefault()?.RoleId).Name;
            var data = new AccountManageRoleViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = (AccountManageRoleViewModel.RoleEnum) Enum.Parse(typeof(AccountManageRoleViewModel.RoleEnum),
                    oldRole)
            };
            return View(data);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ChangeRole(AccountManageRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //var user = DbContext.Users.Find(model.Id);

            var oldUser = UserManager.FindById(model.Id);
            if (oldUser == null) return HttpNotFound();

            var oldRole = oldUser.Roles.SingleOrDefault();
            if (oldRole == null) return HttpNotFound();

            var oldRoleId = oldRole.RoleId;
            var oldRoleNameCheckNull = DbContext.Roles.SingleOrDefault(r => r.Id == oldRoleId);
            if (oldRoleNameCheckNull == null) return HttpNotFound();

            var oldRoleName = oldRoleNameCheckNull.Name;
            if (oldRoleName == model.Role.ToString()) return RedirectToAction("Index");

            UserManager.RemoveFromRole(model.Id, oldRoleName);
            UserManager.AddToRole(model.Id, model.Role.ToString());
            return RedirectToAction("Index");
        }
    }
}