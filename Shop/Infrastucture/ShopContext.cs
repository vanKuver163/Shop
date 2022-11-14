using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Infrastucture
{
    public class ShopContext: IdentityDbContext<AppUser>
    {
        public ShopContext(DbContextOptions<ShopContext> options) :base(options)
        {
        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
