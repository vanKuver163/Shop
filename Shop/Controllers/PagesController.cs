using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastucture;
using Shop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class PagesController : Controller
    {
        private readonly ShopContext _db;
        public PagesController(ShopContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await _db.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await _db.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}
