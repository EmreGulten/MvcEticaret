using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebUI.Entity;
using MVCWebUI.Models;

namespace MVCWebUI.Controllers
{
    public class HomeController : Controller
    {

        DataContext _context = new DataContext();

        // GET: Home
        public ActionResult Index()
        {
            var urunler = _context.Products.Where(x => x.IsApproved && x.IsHome).Select(x => new ProductModel()
            {
                Id = x.Id,
                Name = x.Name.Length > 45 ? x.Name.Substring(0, 42) + "..." : x.Name,
                Description = x.Description.Length > 50 ? x.Description.Substring(0, 47) + "..." : x.Description,
                Price = x.Price,
                Stock = x.Stock,
                Image = x.Image ?? "1.jpeg",
                CategoryId = x.CategoryId
            }).ToList();

            return View(urunler);
        }

        public ActionResult Details(int id)
        {
            return View(_context.Products.Where(x => x.Id == id).FirstOrDefault());
        }

        public ActionResult List(int? id)
        {
            var urunler = _context.Products.Where(x => x.IsApproved).Select(x => new ProductModel()
            {
                Id = x.Id,
                Name = x.Name.Length > 45 ? x.Name.Substring(0, 42) + "..." : x.Name,
                Description = x.Description.Length > 50 ? x.Description.Substring(0, 47) + "..." : x.Description,
                Price = x.Price,
                Stock = x.Stock,
                Image = x.Image,
                CategoryId = x.CategoryId
            }).AsQueryable();

            if (id != null)
            {
                urunler = urunler.Where(x => x.CategoryId == id);
            }

            return View(urunler.ToList());
        }


        public PartialViewResult GetCatecories()
        {
            var kategori = _context.Categories.ToList();

            return PartialView(kategori);
        }

        

    }
}