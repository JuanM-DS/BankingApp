using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;

namespace BankingApp.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> UpdateAsync(ApplicationUserDTO userDto);

        public Task<bool> DeleteAsycn(ApplicationUserDTO userDto);

        public Task<ApplicationUserDTO> GetAsync(string id);

        public Task<IEnumerable<ApplicationUserDTO>> Get();

        public Task<IEnumerable<ApplicationUserDTO>> GetAsync(RoleTypes role);
    }
}
