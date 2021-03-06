﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Flora_Queen_Project.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private RoleManager<ApplicationUserRole> _roleManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db;

        public AccountController()
        {
        }

        public AccountController(
            RoleManager<ApplicationUserRole> roleManager,
            ApplicationUserManager userManager, 
            ApplicationSignInManager signInManager, 
            ApplicationDbContext dbContext 
            )
        {
            RoleManager = roleManager;
            DbContext = dbContext;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public RoleManager<ApplicationUserRole> RoleManager
        {
            get => _roleManager ?? new RoleManager<ApplicationUserRole>(new RoleStore<ApplicationUserRole>(DbContext));
            private set => _roleManager = value;
        }
        public ApplicationDbContext DbContext
        {
            get => _db ?? ApplicationDbContext.Create();
            private set => _db = value;
        }
        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var firstOrDefault = DbContext.Users.FirstOrDefault(p => p.Email == model.Email);
            if (firstOrDefault == null)
            {
                ModelState.AddModelError("", @"Username not found or wrong password.");
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(firstOrDefault.UserName, model.Password, model.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                case SignInStatus.Failure:
                    ModelState.AddModelError("", @"Username not found or wrong password.");
                    return View(model);
                default:
                    ModelState.AddModelError("", @"Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            model.RememberMe = true;
            model.RememberBrowser = true;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                    return View(model);
                default:
                    ModelState.AddModelError("", @"Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, "User");
                await SignInManager.SignInAsync(user, true, true);
                    
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }



            // Sign in the user with this external login provider if the user already has a login

            var result = await SignInManager.ExternalSignInAsync(loginInfo, true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = true });
                // ReSharper disable once RedundantCaseLabel
                case SignInStatus.Failure:
                    ViewBag.FailureInfo = "Email has already taken";
                    return View("ExternalLoginFailure");
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                    {
                        Username = loginInfo.Email, Email = loginInfo.Email
                    });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            Debug.WriteLine(returnUrl);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                if (await UserManager.FindByNameAsync(model.Username) != null)
                {
                    ViewBag.Duplicate = "Username used, please try another";
                    return View(model);
                }

                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, true, true);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
 
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxCheckUserName(string username)
        {
            var validationResult = await UserManager.FindByNameAsync(username) == null;
            Debug.WriteLine(validationResult);
            return Json(validationResult);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string currentUrl)
        {
            if (!string.IsNullOrEmpty(currentUrl))
            {
                Debug.WriteLine(currentUrl);
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return Redirect(currentUrl);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        public ActionResult MyOrder(int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }
            var id = User.Identity.GetUserId();
            var listOrder = _db.ApplicationOrders.Where(o => o.ApplicationUserId == id).OrderByDescending(o=>o.CreatedAt).ToList();
           
            var mylistOrder = new List<MyOrderViewModel>();
            for (int i = 0; i < listOrder.Count; i++)
            {
                mylistOrder.Add(new MyOrderViewModel
                {
                    OrderStatus = listOrder[i].OrderStatus,
                    CreatedAt = listOrder[i].CreatedAt,
                    Amount = listOrder[i].Amount,
                    PaymentMethod = listOrder[i].PaymentMethod,
                    BankCode = listOrder[i].BankCode,
                    OrderDescription = listOrder[i].OrderDescription,
                    ShipName = listOrder[i].ShipName,
                    ShipPhone = listOrder[i].ShipPhone,
                    ShipAddress = listOrder[i].ShipAddress,
                    ShipEmail = listOrder[i].ShipEmail,
                    vpn_Message = listOrder[i].vpn_Message,
                    vpn_TxnResponseCode = listOrder[i].vpn_TxnResponseCode,
                    vnp_TransactionNo = listOrder[i].vnp_TransactionNo,
                    Id = listOrder[i].Id
                });
            }

            ViewBag.TotalPage = Math.Ceiling((double)mylistOrder.Count / limit.Value);
            ViewBag.CurrentPage = page;
            ViewBag.Limit = limit;

            mylistOrder = mylistOrder.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            return View(mylistOrder);
        }

        public async Task<ActionResult> MyOrderDetail(string orderId)
        {
            var order = await _db.ApplicationOrders.FindAsync(orderId);
            if (order == null)
            {
                return HttpNotFound();
            }
            var listOrderItem = await _db.OrderItems.Where(od => od.OrderId == orderId).ToListAsync();

            var myOrder = new MyOrderViewModel
            {
                OrderStatus = order.OrderStatus,
                CreatedAt = order.CreatedAt,
                Amount = order.Amount,
                PaymentMethod = order.PaymentMethod,
                BankCode = order.BankCode,
                OrderDescription = order.OrderDescription,
                ShipName = order.ShipName,
                ShipPhone = order.ShipPhone,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                vpn_Message = order.vpn_Message,
                vpn_TxnResponseCode = order.vpn_TxnResponseCode,
                vnp_TransactionNo = order.vnp_TransactionNo,
                Id = order.Id
            };

            var listMyOderItem = new List<MyOrderItemViewModel>();
            for (int i = 0; i < listOrderItem.Count; i++)
            {
                listMyOderItem.Add(new MyOrderItemViewModel
                {
                    ImgUrl = listOrderItem[i].Product.ImgUrl,
                    Discount = listOrderItem[i].Discount,
                    Quantity = listOrderItem[i].Quantity,
                    UnitPrice = listOrderItem[i].UnitPrice
                });
            }

            ViewBag.listMyOderItem = listMyOderItem;
            return View(myOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}