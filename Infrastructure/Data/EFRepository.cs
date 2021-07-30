using ApplicationCore.Interfaces;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : class, IAggregateRoot
    {
        protected readonly IDbContextFactory<MagShopContext> _contextFactory;

        public EfRepository(IDbContextFactory<MagShopContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var specificationResult = ApplySpecification(spec);
                return await specificationResult.ToListAsync();
            }
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var specificationResult = ApplySpecification(spec);
                return await specificationResult.CountAsync();
            }
        }

        public async Task<T> AddAsync(T entity, CancellationToken token)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync(token);

                return entity;
            }
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync(token);
            }
        }

        public async Task DeleteAsync(T entity, CancellationToken token)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync(token);
            }
        }
        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var specificationResult = ApplySpecification(spec);
                return await specificationResult.FirstAsync();
            }
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var specificationResult = ApplySpecification(spec);
                return await specificationResult.FirstOrDefaultAsync();
            }
        }
        protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var evaluator = new SpecificationEvaluator<T>();
                return evaluator.GetQuery(context.Set<T>().AsSplitQuery(), spec);
            }
        }
    }
}
