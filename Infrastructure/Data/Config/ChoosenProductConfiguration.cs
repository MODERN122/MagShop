using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class ChoosenProductConfiguration : IEntityTypeConfiguration<ChoosenProduct>
    {
        public void Configure(EntityTypeBuilder<ChoosenProduct> builder)
        {
            builder.HasOne(x=>x.Product)
                .WithMany(x=>x.ChoosenProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
