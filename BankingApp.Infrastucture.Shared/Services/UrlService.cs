using BankingApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace BankingApp.Infrastructure.Shared.Services
{
    public class UrlService(string origin) : IUrlService
    {
        private readonly string _origin = origin;

        public string GetConfimrEmailUrl(string token, string email)
        {
            var route = "Login/ConfirmEmail";

            var uri = new Uri(string.Concat(origin, route));
            var finalUrl = QueryHelpers.AddQueryString(uri.ToString(), "Token", token);
            finalUrl = QueryHelpers.AddQueryString(finalUrl, "Email", email);
            return finalUrl;
        }

        public string GetResetPasswordUrl(string token, string email)
        {
            var route = "Login/ResetPassword";

            var uri = new Uri(string.Concat(origin, route));
            var finalUrl = QueryHelpers.AddQueryString(uri.ToString(), "Token", token);
            finalUrl = QueryHelpers.AddQueryString(finalUrl, "Email", email);
            return finalUrl;
        }
    }
}
