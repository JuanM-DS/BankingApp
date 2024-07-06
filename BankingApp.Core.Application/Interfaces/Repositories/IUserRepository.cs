using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> UpdateAsync(ApplicationUserDTO userDto);

        public Task<bool> DeleteAsycn(ApplicationUserDTO userDto);

        public Task<ApplicationUserDTO> GetAsync(string id);

        public IQueryable<ApplicationUserDTO> Get();

        public Task<IQueryable<ApplicationUserDTO>> GetAsync(RoleTypes role);
        public Task<ApplicationUserDTO> GetUserByUserName(string userName);
    }
}
