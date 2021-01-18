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

            builder.HasOne(ho => ho.Basket)
                .WithOne()
                .HasForeignKey<Basket>(x=>x.UserId)
                .IsRequired();
            builder.HasMany(x => x.Stores)
                .WithOne(x => x.Seller)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            //builder.OwnsMany(p => p.Addresses, u =>
            //{
            //    u.WithOwner();
            //    u.HasKey(p => p.AddressId);
            //    u.Property(p => p.City)
            //    .IsRequired();
            //});

            //builder.OwnsOne(ot => ot.UserAuthAccess, u =>
            //{
            //    u.WithOwner();
            //    u.Property(p => p.Password)
            //    .HasMaxLength(100)
            //    .IsRequired();
            //    u.Property(p => p.UserName)
            //    .HasMaxLength(100)
            //    .IsRequired();
            //    u.HasIndex(p => p.UserName);
            //    u.HasIndex(p => p.Password).IsUnique();
            //    u.HasIndex(p => p.GoogleToken).IsUnique();
            //    u.HasIndex(p => p.FacebookToken).IsUnique();
            //    u.HasIndex(p => p.FirebaseToken).IsUnique();
            //    u.HasIndex(p => p.OauthToken).IsUnique();
            //});

            builder.Property(p => p.FavoriteProductsId)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
