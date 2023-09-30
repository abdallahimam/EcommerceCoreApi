using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> specification);

        Task<IReadOnlyList<T>> GetListEntityWithSpecAsync(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specification);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}