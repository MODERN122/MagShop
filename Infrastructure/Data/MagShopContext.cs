using ApplicationCore.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<ProductPropertyItem> ProductPropertyItems { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PropertyItem> PropertyItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ChoosenProduct> ChoosenProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dateTimeConverter);
                }
            }

            base.OnModelCreating(builder);
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
                   });
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
