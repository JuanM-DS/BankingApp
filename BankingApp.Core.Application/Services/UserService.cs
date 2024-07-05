using BankingApp.Core.Application.CostomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.User;

namespace BankingApp.Core.Application.Services
{
    internal class UserService : IUserServices
    {
        public Task<Response<SaveUserViewModel>> CreateAsync(SaveUserViewModel userViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserViewModel>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<UserViewModel>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<UserViewModel>>> GetAll(RoleTypes roleType)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserViewModel>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserViewModel>> GetByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SaveUserViewModel>> GetSaveByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SaveUserViewModel>> UpdateAsync(SaveUserViewModel userViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
