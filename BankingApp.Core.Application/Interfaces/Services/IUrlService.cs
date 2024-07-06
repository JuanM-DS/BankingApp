namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IUrlService
    {
        public string GetConfimrEmailUrl(string token, string email);

        public string GetResetPasswordUrl(string token, string email);
    }
}
