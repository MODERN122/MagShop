using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(o => o.ShipToAddress).WithMany().HasForeignKey(k => k.AddressId);
            builder.HasOne(o => o.CreditCard).WithMany().HasForeignKey(k => k.CreditCardId);
            builder.HasOne(o => o.Transaction).WithMany().HasForeignKey(k => k.TransactionId);


            //builder.HasMany(x => x.Items)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.OwnsOne(b => b.ShipToAddress, a =>
            //{
            //    a.WithOwner();

            //    a.Property(a => a.Apartment)
            //        .HasMaxLength(10)
            //        .IsRequired();
            //    a.Property(a => a.City)
            //        .HasMaxLength(100)
            //        .IsRequired();
            //    a.Property(a => a.House)
            //        .HasMaxLength(15)
            //        .IsRequired();
            //    a.Property(a => a.Street)
            //        .HasMaxLength(180)
            //        .IsRequired();
            //    a.Property(a => a.ZipCode)
            //        .HasMaxLength(18)
            //        .IsRequired();
            //});
        }
    }
}
