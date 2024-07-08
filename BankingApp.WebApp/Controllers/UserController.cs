using BankingApp.WebApp.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class UserController : Controller
    {
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
