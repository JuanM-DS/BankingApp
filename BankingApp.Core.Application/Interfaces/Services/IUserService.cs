using BankingApp.Core.Application.CostomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.ViewModels.User;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<Response<SaveUserViewModel>> CreateAsync(SaveUserViewModel userViewModel);

        public Task<Response<SaveUserViewModel>> UpdateAsync(SaveUserViewModel userViewModel);

        public Task<Response<UserViewModel>> DeleteAsync(string id);

        public Task<Response<UserViewModel>> GetByIdAsync(string id);

        public Task<Response<SaveUserViewModel>> GetSaveByIdAsync(string id);

        public Task<Response<UserViewModel>> GetByNameAsync(string userName);

        public Response<IEnumerable<UserViewModel>> GetAll();

        public Task<Response<IEnumerable<UserViewModel>>> GetAll(RoleTypes roleType);
    }
}
