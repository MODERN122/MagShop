using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class ProductPropertyConfiguration : IEntityTypeConfiguration<ProductProperty>
    {
        public void Configure(EntityTypeBuilder<ProductProperty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(p => p.ProductPropertyItems)
                .WithOne(x => x.ProductProperty)
                .HasForeignKey(x => x.ProductPropertyId);
        }
    }
}
