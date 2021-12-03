using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Categories
{
    [ExtendObjectType(typeof(Query))]
    public class CategoryQueries
    {
        private readonly IAsyncRepository<Category> _categoriesRepository;

        public CategoryQueries(
            IAsyncRepository<Category> categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        public async Task<Category> GetCategoryById(string id) =>
            await _categoriesRepository.GetByIdAsync(id);

        //TODO maybe replace to repository
        [UseMagShopContext]
        public async Task<IEnumerable<Category>> GetCategories(string parentId, [ScopedService] MagShopContext context)
        {
            var categories = await context.Categories.Include(x=>x.Childs).Where(x=>x.ParentId==parentId).ToListAsync();
            return categories;
        }
    }
}
