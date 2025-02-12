﻿using AutoMapper;
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
            IEmailService emailServices,
            IMapper mapper,
            IUrlService urlService
        )
        : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IEmailService _emailServices = emailServices;
        private readonly IMapper _mapper = mapper;
        private readonly IUrlService _urlService = urlService;

        public async Task<AuthenticationResponseDTO> AuthenticationAsync(AuthenticationRequestDTO request)
        {
            var userByUserName = await  _userManager.FindByNameAsync(request.UserName);
            if (userByUserName is null)
                return new()
                {
                    Success = false,
                    Error = $"{request.UserName} no esta registrado"
                };

            if(userByUserName.Status is (int)UserStatus.Inactive)
                return new()
                {
                    Success = false,
                    Error = $"{request.UserName} esta inactivo contacta con un admin"
                };

            if (userByUserName.EmailConfirmed is false)
                return new()
                {
                    Success = false,
                    Error = $"{userByUserName.Email} no esta registrado"
                };

            var result = await _signInManager.PasswordSignInAsync(userByUserName, request.Password, false, lockoutOnFailure : false);

            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = "las credenciales no son correctas"
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
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is null)
                return new()
                {
                    Success = false,
                    Error = "el usuario no existe"
                };

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ConfirmEmailAsync(userByEmail, code);

            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = result.Errors.First().Description
                };

            return new()
            {
                Success = true,
            };
        }

        public async Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is null)
                return new()
                {
                    Success = false,
                    Error = $"{request.Email} no esta registrado"
                };

            if (userByEmail.Status is (int)UserStatus.Inactive)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.UserName} esta inactivo contacta con un admin"
                };

            if (userByEmail.EmailConfirmed is false)
                return new()
                {
                    Success = false,
                    Error = $"{userByEmail.Email} no esta confirmado"
                };
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(userByEmail);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var url = _urlService.GetResetPasswordUrl(token, userByEmail.Email);

            var emailRequest = new EmailRequestDTO()
            {
                To = userByEmail.Email,
                Body = $"cambia tu contrasena en la siguiente url: {url}",
                Subject = "Reset your Password"
            };
            await _emailServices.SendEmailAsync(emailRequest);

            return new() { Success = true };
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponseDTO> RegisterAsync(ApplicationUserDTO request)
        {
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName is not null)
                return new()
                {
                    Success = false,
                    Error = $"EL user name: {request.UserName} ya esta tomado"
                };

            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
                return new()
                {
                    Success = false,
                    Error = $"el email: {request.Email} ya esta tomado"
                };

            var userByIdCard = await _userManager.Users.Where(x => x.IdCard == request.IdCard).FirstOrDefaultAsync();
            if (userByIdCard is not null)
                return new()
                {
                    Success = false,
                    Error = $"la cedula: {request.IdCard} ya esta tomada"
                };

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new()
                {
                    Success = false,
                    Error = result.Errors.First().Description
                };

            await userManager.AddToRolesAsync(user, request.Roles.Select(x=>x.ToString()));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var url = _urlService.GetConfimrEmailUrl(token, request.Email);
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
                    Error = $"{request.Email} no esta registrado"
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
    }
}
