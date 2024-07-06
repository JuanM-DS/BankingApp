using AutoMapper;
using BankingApp.Core.Application.CostomEntities;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Core.Application.Services
{
    internal class UserService(IUserRepository userRepository,IAccountService accountService ,IMapper mapper) : IUserService
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
            if(result)
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

        public Response<IEnumerable<UserViewModel>> GetAll()
        {
            var users =  _userRepository.Get().AsEnumerable();

            var userViewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);

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

        public async Task<Response<UserViewModel>> GetByNameAsync(string userName)
        {
            var users =  await _userRepository.Get()
                                              .FirstOrDefaultAsync(x => x.UserName == userName);

            var userViewModels = _mapper.Map<UserViewModel>(users);

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
            var userByUserName = await _userRepository.Get()
                                              .FirstOrDefaultAsync(x => x.UserName == userViewModel.UserName);
            if (userByUserName is not null && userByUserName.Id != userViewModel.Id)
                return new()
                {
                    Success = false,
                    Error = $"{userViewModel.UserName} is already taken"
                };

            var userByEmail = await _userRepository.Get()
                                              .FirstOrDefaultAsync(x => x.Email == userViewModel.Email);
            if (userByEmail is not null && userByUserName.Id != userViewModel.Id)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.Email} is already taken"
                };

            var userByIdCard = await _userRepository.Get()
                                              .FirstOrDefaultAsync(x => x.IdCard == userViewModel.IdCard);
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
    }
}
