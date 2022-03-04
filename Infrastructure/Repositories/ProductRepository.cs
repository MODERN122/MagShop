using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbContextFactory<MagShopContext> dbContext) : base(dbContext)
        {
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var productOld = context.Products
                    .Include(i => i.Category)
                    .Include(x => x.ProductProperties).ThenInclude(x => x.ProductPropertyItems)
                    .Include(x => x.ChoosenProducts).ThenInclude(x => x.PropertyItemTuples)
                    .Include(p => p.Properties)
                    .Include(p => p.Images)
                    .Include(p => p.Store)
                    .Single(c => c.Id == product.Id);

                context.Entry(productOld).CurrentValues.SetValues(product);
                context.Entry(productOld.Category).CurrentValues.SetValues(product.Category);
                context.ProductProperties.RemoveRange(productOld.ProductProperties);
                await context.ProductProperties.AddRangeAsync(product.ProductProperties);
                context.ChoosenProducts.RemoveRange(productOld.ChoosenProducts);
                await context.ChoosenProducts.AddRangeAsync(product.ChoosenProducts);
                context.ProductImages.RemoveRange(productOld.Images);
                await context.ProductImages.AddRangeAsync(product.Images);
                context.Entry(product.Store).CurrentValues.SetValues(product.Store);

                var result = await context.SaveChangesAsync();

                return result > 0;
            }
        }
    }
}
