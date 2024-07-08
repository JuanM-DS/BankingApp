using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Account;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Application.Helpers;
using BankingApp.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Core.Application.Enums;

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
    }
}
