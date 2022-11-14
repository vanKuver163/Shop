using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Shop.Models
{
    public class SmallCartViewModel
    {
        public int NumberOfItem { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
