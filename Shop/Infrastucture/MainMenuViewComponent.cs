using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Infrastucture
{
    public class MainMenuViewComponent: ViewComponent
    {
        private readonly ShopContext _db;
        public MainMenuViewComponent(ShopContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return _db.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
