using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MVCWebUI.Entity;
using MVCWebUI.Identity;
using MVCWebUI.Models;

namespace MVCWebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        private DataContext db = new DataContext();

        public AccountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
            roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new IdentityDataContext()));
        }

        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var order = db.Orders.Where(x => x.Username == username).Select(x => new OrderModel()
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                DateTime = x.DateTime,
                OrderState = x.OrderState,
                Total = x.Total
            }).OrderByDescending(x => x.DateTime).ToList();


            return View(order);
        }


        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = db.Orders.Where(x => x.Id == id).Select(x => new OrderDetailsModel()
            {
                OrderId = x.Id,
                OrderNumber = x.OrderNumber,
                Total = x.Total,
                DateTime = x.DateTime,
                OrderState = x.OrderState,
                AdresBasligi = x.AdresBasligi,
                Adres = x.Adres,
                Sehir = x.Sehir,
                Semt = x.Semt,
                Mahalle = x.Mahalle,
                PostaKodu = x.PostaKodu,
                OrderLines = x.OrderLines.Select(a => new OrderLineModel()
                {
                    ProductId = a.ProductId,
                    ProductName = a.Product.Name.Length > 50 ? a.Product.Name.Substring(0, 47) + "..." : a.Product.Name,
                    Image = a.Product.Image,
                    Quantity = a.Quantity,
                    Price = a.Price
                }).ToList()


            }).FirstOrDefault();

            return View(entity);
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

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.UserName, model.Password);


                if (user != null)
                {
                    var autManager = HttpContext.GetOwinContext().Authentication;
                    var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe;


                    autManager.SignIn(authProperties, identity);

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