using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Account;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Domain.Entities;
using BankingApp.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class LoginController(IUserService userServices, ImageServices imageServices) : Controller
    {
        private readonly IUserService _userServices = userServices;
        private readonly ImageServices _imageServices = imageServices;

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel login)
        {
            if (!ModelState.IsValid) return View(login);
            
            var result = await _userServices.LoginAsync(login);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View(login);
            }

            HttpContext.Session.Set<UserViewModel>("user", result.Data);

            if (result.Data.Roles.Contains(RoleTypes.Admin))
                return RedirectToAction("Index", "Admin");

            if (result.Data.Roles.Contains(RoleTypes.Client))
                return RedirectToAction("Index", "Client");

            return View(login);
        }

        public async Task<IActionResult> ConfirmAccount(ConfirmAccountViewModel viewModel)
        {
            var result = await _userServices.ConfirmAccountAsync(viewModel);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
               
                return View(viewModel);
            }

            ViewData["Message"] = $"Account confirm for {viewModel.Email}, You can now user the app";

            return View();
        }

        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var result = await _userServices.ForgotPasswordAsync(viewModel);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View(viewModel);
            }

            return View(nameof(Index));
        }

        public IActionResult ResetPassword(string Token, string Email) => View(new ResetPasswordViewModel() { Email = Email, Token = Token});

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var result = await _userServices.ResetPasswordAsync(viewModel);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View(viewModel);
            }

            return View(nameof(Index));
        }

        public async Task<IActionResult> LogOut()
        {
            await _userServices.SingOutAsync();
            HttpContext.Session.Remove("user");

            return View(nameof(Index));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
