using ApplicationCore.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Data
{
    public class MagShopContext : IdentityDbContext<UserAuthAccess>
    {
        public MagShopContext(DbContextOptions<MagShopContext> options) : base(options)
        {
        }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            builder.Entity<Product>()
           .HasMany(p => p.Properties)
           .WithMany(p => p.Products)
           .UsingEntity<ProductProperty>(
               j => j
                   .HasOne(pt => pt.Property)
                   .WithMany(t => t.ProductProperties)
                   .HasForeignKey(pt => pt.PropertyId),
               j => j
                   .HasOne(pt => pt.Product)
                   .WithMany(p => p.ProductProperties)
                   .HasForeignKey(pt => pt.ProductId),
               j =>
               {
                   j.Property(pt => pt.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                   j.HasKey(t => new { t.ProductId, t.PropertyId });
               });
        }
    }
}
