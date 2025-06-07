using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TutorialProjectAPI.Repositories
{
    /// <summary>
    /// Generic repository interface for any entity that has a Guid Id.
    /// </summary>
    public interface IIdentifiableRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
