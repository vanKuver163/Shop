using Microsoft.AspNetCore.Mvc;
using Shop.Infrastucture;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ShopContext _db;
        public ProductsController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = _db.Products.OrderByDescending(x => x.Id).Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_db.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            Category category = await _db.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");


            int pageSize = 6;
            var products = _db.Products.OrderByDescending(x => x.Id).Where(x=>x.CategoryId==category.id).Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_db.Products.Where(x => x.CategoryId == category.id).Count() / pageSize);

            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = category.Slug;

            return View(await products.ToListAsync());
        }
    }
}
