using AutoMapper;
using BankingApp.Core.Application.CostomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.User;

namespace BankingApp.Core.Application.Services
{
    internal class UserService(IUserRepository userRepository,IAccountService accountService ,IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<SaveUserViewModel>> CreateAsync(SaveUserViewModel userViewModel)
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
