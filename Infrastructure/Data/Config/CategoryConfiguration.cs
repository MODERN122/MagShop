using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(b => b.ParentId)
                .IsRequired(false);
            builder.HasOne(p => p.ParentCategory)
                .WithMany(m => m.Childs)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(f => f.ParentId);
        }
    }
}
