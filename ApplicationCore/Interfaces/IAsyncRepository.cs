using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : IAggregateRoot
    {
        Task<T> GetByIdAsync(string id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity, CancellationToken token= default);
        Task<bool> UpdateEntryAsync(T entity, CancellationToken token = default);
        Task<bool> UpdateFieldsAsync(T entity, CancellationToken token = default, params Expression<Func<T, object>>[] includeProperties);
        Task DeleteAsync(T entity, CancellationToken token = default);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    }
}
