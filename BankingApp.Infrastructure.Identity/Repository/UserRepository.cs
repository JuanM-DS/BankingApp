using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Infrastructure.Identity.Contexts;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Infrastructure.Identity.Repository
{
    public class UserRepository(BankingAppIdentityDbContext context, IMapper mapper) : IUserRepository
    {
        private readonly BankingAppIdentityDbContext context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> DeleteAsycn(ApplicationUserDTO userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);

            try
            {
                context.Users.Remove(user);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<ApplicationUserDTO> Get()
        {
            var users = context.Users.AsQueryable();

            return _mapper.Map<IQueryable<ApplicationUserDTO>>(users);
        }

        public async Task<ApplicationUserDTO> GetAsync(int id)
        {
            var user = await context.Users.FindAsync(id);

            var userDTo = _mapper.Map<ApplicationUserDTO>(user);
            
            return userDTo;
        }

        public async Task<IQueryable<ApplicationUserDTO>> GetAsync(RoleTypes roleType)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == roleType.ToString());

            var UserIds = context.UserRoles.Where(x => x.RoleId == role.Id)
                                           .Select(x => x.UserId)
                                           .ToList();

            if (UserIds is null)
                return Enumerable.Empty<ApplicationUserDTO>().AsQueryable();

            var users = context.Users.Where(x => UserIds.Contains(x.Id));

            return _mapper.Map<IQueryable<ApplicationUserDTO>>(users);
        }

        public async Task<ApplicationUserDTO> GetAsync(string id)
        {
            var user = await context.Users.FindAsync(id);
            var userDto = _mapper.Map<ApplicationUserDTO>(user);
            return userDto;
        }

        public async Task<bool> UpdateAsync(ApplicationUserDTO userDto)
        {
            var user = await context.Users.FindAsync(userDto.Id);

            _mapper.Map(userDto, user);

            try
            {
                context.Users.Update(user);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
