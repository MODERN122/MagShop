using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class ProductPropertyItemConfiguration : IEntityTypeConfiguration<ProductPropertyItem>
    {
        public void Configure(EntityTypeBuilder<ProductPropertyItem> builder)
        {
            builder.HasOne(x => x.PropertyItem)
                .WithMany(x => x.ProductPropertyItems)
                .HasForeignKey(x => x.PropertyItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
