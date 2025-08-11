using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Contracts.Persistence
{
    public interface IBaseRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> AddListAsync(List<T> entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdGuidAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> UpdateAsync(T entity);
    }
}
