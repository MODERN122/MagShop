﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(x => x.Images)
                .WithOne(x=>x.Product)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.ProductProperties)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Cascade);
                        
        }
    }
}
