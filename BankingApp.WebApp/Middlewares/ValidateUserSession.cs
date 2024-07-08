using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.Helpers;

namespace BankingApp.WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponseDTO userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponseDTO>("user");

            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}
