using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankingApp.WebApp.Middlewares
{
    public class LoginAuthorize(IHttpContextAccessor accessor) : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor accessor = accessor;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = accessor.HttpContext.Session.Get<UserViewModel>("user");
            if (currentUser is not null)
            {
                var controller = (Controller)context.Controller;
                var controllerResult = currentUser.Roles.Contains(RoleTypes.Admin) ? "Admin" : "Client";
                context.Result = controller.RedirectToAction("Index", controllerResult);
            }
            else
            {
                await next();
            }
        }
    }
}
