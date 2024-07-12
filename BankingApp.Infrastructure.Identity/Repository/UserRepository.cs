using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Infrastructure.Identity.Contexts;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BankingApp.Infrastructure.Identity.Repository
{
    public class UserRepository(BankingAppIdentityDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : IUserRepository
    {
        private readonly BankingAppIdentityDbContext context = context;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly UserManager<ApplicationUser> userManager = userManager;

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

        public IEnumerable<ApplicationUserDTO> Get()
        {
            var users = context.Users.AsQueryable();

            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(users);
        }
        public async Task<IEnumerable<ApplicationUserDTO>> GetAll()
        {
            var users = context.Users.AsQueryable();
            List<ApplicationUserDTO> usersDTO = [];

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var rolesEnums = roles.Select(x => (RoleTypes)Enum.Parse(typeof(RoleTypes),x));
                var userDTO = _mapper.Map<ApplicationUserDTO>(user);
                userDTO.Roles = rolesEnums.ToList();

                usersDTO.Add(userDTO);
            }

            return usersDTO;
        }



        public async Task<ApplicationUserDTO> GetAsync(int id)
        {
            var user = await context.Users.FindAsync(id);

            var userDTo = _mapper.Map<ApplicationUserDTO>(user);
            
            return userDTo;
        }

        public async Task<IEnumerable<ApplicationUserDTO>> GetAsync(RoleTypes roleType)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == roleType.ToString());

            var UserIds = context.UserRoles.Where(x => x.RoleId == role.Id)
                                           .Select(x => x.UserId)
                                           .ToList();

            if (UserIds is null)
                return Enumerable.Empty<ApplicationUserDTO>().AsQueryable();

            var users = context.Users.Where(x => UserIds.Contains(x.Id));

            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(users);
        }

        public async Task<ApplicationUserDTO> GetAsync(string id)
        {
            var user = await context.Users.FindAsync(id);
            var userDto = _mapper.Map<ApplicationUserDTO>(user);
            var role = await userManager.GetRolesAsync(user);
            var roleEnum = role.Select(x => (RoleTypes)Enum.Parse(typeof(RoleTypes), x));
            userDto.Roles = roleEnum.ToList();
            return userDto;
        }

        public async Task<bool> UpdateAsync(ApplicationUserDTO userDto)
        {
            var user = await context.Users.FindAsync(userDto.Id);
            string photoUrl = "";
            if (user.PhotoUrl != null)
            {
                photoUrl = user.PhotoUrl;
            }
            if(userDto.PhotoUrl != null)
            {
                photoUrl = userDto.PhotoUrl;
            }
            bool emailConfirmed = user.EmailConfirmed;

            _mapper.Map(userDto, user);
            
            try
            {
                user.PhotoUrl = photoUrl;
                user.EmailConfirmed = emailConfirmed;
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
