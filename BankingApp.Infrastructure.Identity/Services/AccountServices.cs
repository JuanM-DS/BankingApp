using AutoMapper;
using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.DTOs.Account.ConfirmAccount;
using BankingApp.Core.Application.DTOs.Account.ForgotPassword;
using BankingApp.Core.Application.DTOs.Account.Register;
using BankingApp.Core.Application.DTOs.Account.ResetPassword;
using BankingApp.Core.Application.DTOs.Email;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankingApp.Infrastructure.Identity.Services
{
    public class AccountServices 
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailServices emailServices,
            IMapper mapper
        )
        : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IEmailServices _emailServices = emailServices;
        private readonly IMapper _mapper = mapper;

        public async Task<AuthenticationResponseDTO> AuthenticationAsync(AuthenticationRequestDTO request)
        {
            var userByUserName = await  _userManager.FindByNameAsync(request.UserName);
            if (userByUserName is null)
                return new()
                {
                    Success = false,
                    Error = $"{request.UserName} is not register"
                };

            if(userByUserName.Status is (int)UserStatus.inactive)
                return new()
                {
                    Success = false,
                    Error = $"{request.UserName} is inactive must contact an administrator"
                };

            if (userByUserName.EmailConfirmed is false)
                return new()
                {
                    Success = false,
                    Error = $"{userByUserName.Email} is not confirmed"
                };

            var result = await _signInManager.PasswordSignInAsync(userByUserName, request.Password, false, lockoutOnFailure : false);

            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = "Credentials are incorrect"
                };

            var userDTO = _mapper.Map<ApplicationUserDTO>(userByUserName);

            var roles = (await _userManager.GetRolesAsync(userByUserName).ConfigureAwait(false)).ToList();
            userDTO.Roles = roles.ConvertAll(role => (RoleTypes)Enum.Parse(typeof(RoleTypes), role));

            return new()
            {
                Success = true,
                UserDTO = userDTO
            };
        }

        public async Task<ConfirmAccountResponseDTO> ConfirmAccountAsync(ConfirmAccountRequestDTO request)
        {
            var userById = await _userManager.FindByIdAsync(request.UserId);
            if (userById is null)
                return new()
                {
                    Success = false,
                    Message = "User doestn exists"
                };

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ConfirmEmailAsync(userById, code);

            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Message = result.Errors.First().Description
                };

            return new()
            {
                Success = true,
                Message = $"Account confirm for {userById.Email}, You can now user the app"
            };
        }

        public async Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is null)
                return new()
                {
                    Success = false,
                    Error = $"{request.Email} is not register"
                };

            if (userByEmail.Status is (int)UserStatus.inactive)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.UserName} is inactive must contact an administrator"
                };

            if (userByEmail.EmailConfirmed is false)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.Email} is not confirmed"
                };

            var url = await GetResetPasswordUrlAsync(userByEmail, request.Origin);

            var emailRequest = new EmailRequestDTO()
            {
                To = userByEmail.Email,
                Body = $"Please reset your account visiting this URl {url}",
                Subject = "Reset your Password"
            };
            await _emailServices.SendEmailAsync(emailRequest);

            return new() { Success = true };
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponseDTO> RegisterAsync(ApplicationUserDTO request, string origin)
        {
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName is not null)
                return new()
                {
                    Success = false,
                    Error = $"{request.UserName} is already taken"
                };

            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
                return new()
                {
                    Success = false,
                    Error = $"{request.Email} is already taken"
                };

            var userByIdCard = await _userManager.Users.Where(x => x.IdCard == request.IdCard).FirstOrDefaultAsync();
            if (userByIdCard is not null)
                return new()
                {
                    Success = false,
                    Error = $"{request.IdCard} is already taken"
                };

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = result.Errors.First().Description
                };

            await userManager.AddToRolesAsync(user, request.Roles.Select(x=>x.ToString()));

            var url = await GetConfirmAccountUrlAsync(user, origin);
            var email = new EmailRequestDTO()
            {
                To = user.Email,
                Subject = "Confirm your account",
                Body = $"Please confirm your account visiting this URl {url}"
            };
            await _emailServices.SendEmailAsync(email);

            var userDTO = _mapper.Map<ApplicationUserDTO>(user);

            return new()
            {
                Success = true,
                UserDTO = userDTO
            };
        }

        public async Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is null)
                return new()
                {
                    Success = false,
                    Error = $"{request.Email} is not register"
                };

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ResetPasswordAsync(userByEmail, code, request.Password);
            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = result.Errors.First().Description
                };
            return new() { Success = true };

        }

        private async Task<string> GetResetPasswordUrlAsync(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "Login/ResetPassword";
            var uri = new Uri(string.Concat(origin,"/", route));
            var finalUrl = QueryHelpers.AddQueryString(uri.ToString(), "Email", user.Email);
            finalUrl = QueryHelpers.AddQueryString(finalUrl, "Token",  code);

            return finalUrl;
        }

        private async Task<string> GetConfirmAccountUrlAsync(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "Login/ConfirmAccount";
            var uri = new Uri(string.Concat(origin,"/",route));
            var finalUrl = QueryHelpers.AddQueryString(uri.ToString(), "UserId", user.Id);
            finalUrl = QueryHelpers.AddQueryString(finalUrl, "Token", code);

            return finalUrl;
        }
    }
}
