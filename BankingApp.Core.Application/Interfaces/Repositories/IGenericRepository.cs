using BankingApp.Core.Domain.Common;
using System.Linq.Expressions;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task AddAsync(Entity entity);
        Task UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        IQueryable<Entity> GetAllWithInclude(params Expression<Func<Entity, object>>[] properties);
    }
}
