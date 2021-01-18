using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            //builder.HasOne(o => o.Seller)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasForeignKey(f => f.SellerId);
        }
    }
}
