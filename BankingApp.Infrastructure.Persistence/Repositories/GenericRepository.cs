using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Common;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using BankingApp.Core.Application.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BankingApp.Core.Application.Enums;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : UserAuditableBaseEntity
    {
        private readonly ApplicationContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponseDTO userViewModel;

        public GenericRepository(ApplicationContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponseDTO>("user");
        }

        public virtual async Task AddAsync(Entity entity)
        {
            entity.UserName = userViewModel.UserDTO.UserName;
            await _dbContext.Set<Entity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            var entry = await _dbContext.Set<Entity>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Entity entity)
        {
            entity.UserName = userViewModel.UserDTO.UserName;
            _dbContext.Set<Entity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual IQueryable<Entity> GetAllAsync()
        {
            if (userViewModel.UserDTO.Roles.Contains(RoleTypes.Client))
            {
                return _dbContext.Set<Entity>().Where(x => x.UserName == userViewModel.UserDTO.UserName);
            }

            return _dbContext.Set<Entity>();

        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }

        public IQueryable<Entity> GetAllWithInclude(params Expression<Func<Entity, object>>[] properties)
        {
            IQueryable<Entity> query;
            if (userViewModel.UserDTO.Roles.Contains(RoleTypes.Client))
            {
                query = _dbContext.Set<Entity>().Where(x => x.UserName == userViewModel.UserDTO.UserName).AsQueryable();
            }
            else
            {
                query = _dbContext.Set<Entity>().AsQueryable();
            }
            

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return query;
        }
    }
}
