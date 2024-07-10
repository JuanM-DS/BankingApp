using AutoMapper;
using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.DTOs.Account.ConfirmAccount;
using BankingApp.Core.Application.DTOs.Account.ForgotPassword;
using BankingApp.Core.Application.DTOs.Account.ResetPassword;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Account;
using BankingApp.Core.Application.ViewModels.User;
using System.Data;

namespace BankingApp.Core.Application.Services
{
    public class UserService(IUserRepository userRepository, IAccountService accountService, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<SaveUserViewModel>> CreateAsync(SaveUserViewModel userViewModel)
        {
            var userDto = _mapper.Map<ApplicationUserDTO>(userViewModel);

            var result = await _accountService.RegisterAsync(userDto);
            if (!result.Success)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Data = userViewModel,
                Success = true,
            };
        }

        public async Task<Response<UserViewModel>> DeleteAsync(string id)
        {
            var userById = await userRepository.GetAsync(id);
            if (userById is null)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = $"There is any user with this id: {id}"
                };

            var result = await _userRepository.DeleteAsycn(userById);
            if (result)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = $"There is a problem deleting the user"
                };

            var userViewModel = _mapper.Map<UserViewModel>(userById);
            return new()
            {
                Data = userViewModel,
                Success = true,
            };
        }

        public async Task<Response<IEnumerable<UserViewModel>>> GetAll(UserQueryFilter? filters = null)
        {
            var users = _userRepository.Get();

            if (filters is not null)
            {
                if (filters.Role is not null)
                {
                    users = await _userRepository.GetAsync((RoleTypes)filters.Role);
                    users = users.Where(x => x.Roles.Contains((RoleTypes)filters.Role));
                }
                    
                if (filters.Email is not null)
                    users = users.Where(x => x.Email == filters.Email);

                if (filters.IdCard is not null)
                    users = users.Where(x => x.IdCard == filters.IdCard);

                if (filters.Status is not null)
                    users = users.Where(x => x.Status == filters.Status);
            }

            var userViewModels = _mapper.Map<IEnumerable<UserViewModel>>(users.AsEnumerable());

            return new()
            {
                Data = userViewModels,
                Success = true
            };
        }

        public async Task<Response<IEnumerable<UserViewModel>>> GetAll(RoleTypes roleType)
        {
            var users = await _userRepository.GetAsync(roleType);

            var userViewModels = _mapper.Map<IEnumerable<UserViewModel>>(users.AsEnumerable());

            return new()
            {
                Data = userViewModels,
                Success = true
            };
        }

        public async Task<Response<UserViewModel>> GetByIdAsync(string id)
        {
            var users = await _userRepository.GetAsync(id);

            var userViewModels = _mapper.Map<UserViewModel>(users);

            return new()
            {
                Data = userViewModels,
                Success = true
            };
        }

        public Response<UserViewModel> GetByNameAsync(string userName)
        {
            var user = _userRepository.Get()
                                      .FirstOrDefault(x => x.UserName == userName);

            var userViewModels = _mapper.Map<UserViewModel>(user);

            return new()
            {
                Data = userViewModels,
                Success = true
            };
        }

        public async Task<Response<SaveUserViewModel>> GetSaveByIdAsync(string id)
        {
            var users = await _userRepository.GetAsync(id);

            var userViewModels = _mapper.Map<SaveUserViewModel>(users);

            return new()
            {
                Data = userViewModels,
                Success = true
            };
        }

        public async Task<Response<SaveUserViewModel>> UpdateAsync(SaveUserViewModel userViewModel)
        {
            var userByUserName = _userRepository.Get()
                                              .FirstOrDefault(x => x.UserName == userViewModel.UserName);
            if (userByUserName is not null && userByUserName.Id != userViewModel.Id)
                return new()
                {
                    Success = false,
                    Error = $"{userViewModel.UserName} is already taken"
                };

            var userByEmail = _userRepository.Get()
                                              .FirstOrDefault(x => x.Email == userViewModel.Email);
            if (userByEmail is not null && userByUserName.Id != userViewModel.Id)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.Email} is already taken"
                };

            var userByIdCard = _userRepository.Get()
                                              .FirstOrDefault(x => x.IdCard == userViewModel.IdCard);
            if (userByIdCard is not null && userByUserName.Id != userViewModel.Id)
                return new()
                {
                    Success = false,
                    Error = $"{userByIdCard.IdCard} is already taken"
                };

            var userDto = _mapper.Map<ApplicationUserDTO>(userViewModel);

            var result = await _userRepository.UpdateAsync(userDto);
            if (!result)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = $"There is a problem updating the user: {userViewModel.UserName}"
                };

            return new()
            {
                Data = userViewModel,
                Success = true
            };

        }

        public async Task<Response<UserViewModel>> LoginAsync(LoginViewModel login)
        {
            var userDto = _mapper.Map<AuthenticationRequestDTO>(login);

            var result = await _accountService.AuthenticationAsync(userDto);
            if (!result.Success)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Data = _mapper.Map<UserViewModel>(result.UserDTO),
                Success = true
            };
        }

        public async Task<Response<SaveUserViewModel>> RegisterAsync(SaveUserViewModel userViewModel)
        {
            var userDto = _mapper.Map<ApplicationUserDTO>(userViewModel);

            var result = await _accountService.RegisterAsync(userDto);
            if (!result.Success)
                return new()
                {
                    Data = null,
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Data = _mapper.Map<SaveUserViewModel>(result.UserDTO),
                Success = true
            };
        }

        public async Task SingOutAsync()
        {
            await _accountService.LogOutAsync();
        }
        
        public async Task<Response<bool>> ForgotPasswordAsync(ForgotPasswordViewModel viewModel)
        {
            var request = _mapper.Map<ForgotPasswordRequestDTO>(viewModel);
            var result = await _accountService.ForgotPasswordAsync(request);
            if (!result.Success)
                return new()
                {
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Success = true
            };
        }

        public async Task<Response<bool>> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            var request = _mapper.Map<ResetPasswordRequestDTO>(viewModel);
            var result = await _accountService.ResetPasswordAsync(request);
            if (!result.Success)
                return new()
                {
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Success = true
            };
        }

        public async Task<Response<bool>> ConfirmAccountAsync(ConfirmAccountViewModel viewModel)
        {
            var request = _mapper.Map<ConfirmAccountRequestDTO>(viewModel);
            var result = await _accountService.ConfirmAccountAsync(request);
            if (!result.Success)
                return new()
                {
                    Success = false,
                    Error = result.Error
                };

            return new()
            {
                Success = true
            };
        }
    }
}
