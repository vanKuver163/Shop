using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastucture;
using Shop.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(ShopContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = _db.Products.OrderByDescending(x => x.Id).Include(x => x.Category).Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_db.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product product = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories.OrderBy(x => x.Sorting), "id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(_db.Categories.OrderBy(x => x.Sorting), "id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");                

                var slug = await _db.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The product already exists.");
                    return View(product);
                }
                string imageName = "noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                product.Image = imageName;

                _db.Add(product);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The product has been added!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_db.Categories.OrderBy(x => x.Sorting), "id", "Name", product.CategoryId);

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(_db.Categories.OrderBy(x => x.Sorting), "id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await _db.Products.Where(x=> x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The product already exists.");
                    return View(product);
                }                
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }                

                _db.Update(product);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The product has been edited!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["Error"] = "The product has been deleted!";

            }
            return RedirectToAction("Index");
        }

    }
}
