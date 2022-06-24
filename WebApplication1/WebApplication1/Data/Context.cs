
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;


namespace WebApplication1.Data
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderProduct> orderProducts { get; set; }
        public DbSet<Parameters> parameters { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<PropertyParameter> PropertyParameters { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }


       
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
