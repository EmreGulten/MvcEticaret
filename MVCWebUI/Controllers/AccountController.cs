using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MVCWebUI.Identity;
using MVCWebUI.Models;

namespace MVCWebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public AccountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
            roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new IdentityDataContext()));
        }


        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.Name = model.Name;
                user.Surname = model.SurName;
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    if (roleManager.RoleExists("user"))
                    {
                        userManager.AddToRole(user.Id, "user");
                    }

                    return RedirectToAction("Login");
                }
                else
                {

                    ModelState.AddModelError("Hatalı üyelik", "Kullanıcı oluşturma Hatası");

                    //foreach (var resultError in result.Errors)
                    //{
                    //    ModelState.AddModelError("",resultError);
                    //}
                }

            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

      

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.UserName, model.Password);

                var deneme = User.Identity.Name;

                if (user != null)
                {
                    var autManager = HttpContext.GetOwinContext().Authentication;
                    var identity = userManager.CreateIdentity(user, "ApplicationCokie");
                    var autProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    autManager.SignIn(autProperties, identity);

                    TempData["gonder"] = user;




                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        Redirect(ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");



                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı yada Şifre yanlış");
                }


            }

            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("index", "Home");
        }

    }
}