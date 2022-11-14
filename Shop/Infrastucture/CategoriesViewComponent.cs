using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Infrastucture
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ShopContext _db;
        public CategoriesViewComponent(ShopContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var caregories = await GetCategoriesAsync();
            return View(caregories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return _db.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
