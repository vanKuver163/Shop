using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Infrastucture
{
    public class SmallCartViewComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartVM;
            if(cart==null || cart.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new SmallCartViewModel
                {
                    NumberOfItem = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }
            return View(smallCartVM);
        }
    }
}
