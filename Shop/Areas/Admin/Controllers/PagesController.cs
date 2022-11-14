using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Shop.Infrastucture;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ShopContext _db;
        public PagesController(ShopContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in _db.Pages orderby p.Sorting select p;
            List<Page> pagesList = await pages.ToListAsync();
           

            return View(pagesList);

        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Page page = await _db.Pages.FirstOrDefaultAsync(x => x.Id == id);
            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }
        [HttpGet]
        public IActionResult Create() => View();

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if(ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await _db.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The title already exists.");
                    return View(page);
                }
                _db.Add(page);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The page has been added!";
                return RedirectToAction("Index");
            }
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await _db.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");               
                page.Sorting = 100;

                var slug = await _db.Pages.Where(x=> x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.TryAddModelError("", "The page already exists.");
                    return View(page);
                }
                _db.Update(page);
                await _db.SaveChangesAsync();

                TempData["Success"] = "The page has been edited!";
                return RedirectToAction("Edit", new { id = page.Id});
            }
            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await _db.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                _db.Pages.Remove(page);
                await _db.SaveChangesAsync();
                TempData["Error"] = "The page has been deleted!";

            }
            return RedirectToAction("Index");
        }

        [HttpPost]      
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach(var pageId in id)
            {
                Page page = await _db.Pages.FindAsync(pageId);
                page.Sorting = count;
                _db.Update(page);
                await _db.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
