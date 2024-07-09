using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Account;
using BankingApp.Core.Application.ViewModels.User;
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
    }
}
