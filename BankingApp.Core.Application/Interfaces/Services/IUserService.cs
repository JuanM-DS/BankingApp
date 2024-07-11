using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Account;
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

        public Task<Response<IEnumerable<UserViewModel>>> GetByNameAsync(string filter);

        public Task<Response<IEnumerable<UserViewModel>>> GetAll(UserQueryFilter? filters = null);

        public Task<Response<IEnumerable<UserViewModel>>> GetAll(RoleTypes roleType);

        public Task<Response<UserViewModel>> LoginAsync(LoginViewModel login);

        public Task<Response<SaveUserViewModel>> RegisterAsync(SaveUserViewModel userViewModel);

        public Task SingOutAsync();

        public Task<Response<bool>> ForgotPasswordAsync(ForgotPasswordViewModel viewModel);

        public Task<Response<bool>> ResetPasswordAsync(ResetPasswordViewModel viewModel);

        public Task<Response<bool>> ConfirmAccountAsync(ConfirmAccountViewModel viewModel);
    }
}
