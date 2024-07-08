using BankingApp.Core.Application.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(LoginViewModel login)
        {
            return View(login);
        }
    }
}
