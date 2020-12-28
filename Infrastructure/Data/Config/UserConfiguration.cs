using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var navigation1 = builder.Metadata.FindNavigation(nameof(User.Orders));
            navigation1.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(p => p.Id);

            //builder.OwnsMany(p => p.Addresses, u =>
            //{
            //    u.WithOwner();
            //    u.HasKey(p => p.AddressId);
            //    u.Property(p => p.City)
            //    .IsRequired();
            //});

            builder.OwnsOne(ot => ot.UserAuthAccess, u =>
            {
                u.WithOwner();
                u.Property(p => p.Password)
                .HasMaxLength(100)
                .IsRequired();
                u.Property(p => p.UserName)
                .HasMaxLength(100)
                .IsRequired();
            });

            builder.Property(p => p.FavoriteProductsId)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
