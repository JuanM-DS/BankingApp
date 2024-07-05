using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> UpdateAsync(ApplicationUserDTO userDto);

        public Task<bool> DeleteAsycn(ApplicationUserDTO userDto);

        public IQueryable<ApplicationUserDTO> GetAsync();

        public Task<IQueryable<ApplicationUserDTO>> GetAsync(RoleTypes role);
    }
}
