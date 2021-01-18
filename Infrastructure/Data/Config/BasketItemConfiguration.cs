using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(p => p.BasketId)
                .IsRequired();
            builder.HasOne(x => x.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(x => x.ProductId)
                .IsRequired();
            builder.HasKey(p => p.BasketItemId);
            builder.Property(bi => bi.UnitPrice)
                .IsRequired(true)
                .HasColumnType("decimal(18,2)");
        }
    }
}
