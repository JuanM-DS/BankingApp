﻿using BankingApp.Core.Domain.Common;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : AuditableBaseEntity
    {
        Task AddAsync(Entity entity);
        Task UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
    }
}
