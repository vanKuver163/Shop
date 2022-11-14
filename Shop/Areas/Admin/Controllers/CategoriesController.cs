using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastucture;
using Shop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ShopContext _db;
        public CategoriesController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _db.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await _db.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The category already exists.");
                    return View(category);
                }
                _db.Add(category);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The category has been added!";
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
               
                var slug = await _db.Categories.Where(x => x.id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The category already exists.");
                    return View(category);
                }
                _db.Update(category);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The category has been edited!";
                return RedirectToAction("Edit", new { id });
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exist!";
            }
            else
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                TempData["Error"] = "The page has been deleted!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await _db.Categories.FindAsync(categoryId);
                category.Sorting = count;
                _db.Update(category);
                await _db.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
