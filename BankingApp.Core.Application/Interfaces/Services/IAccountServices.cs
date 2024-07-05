using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.DTOs.Account.ConfirmAccount;
using BankingApp.Core.Application.DTOs.Account.ForgotPassword;
using BankingApp.Core.Application.DTOs.Account.Register;
using BankingApp.Core.Application.DTOs.Account.ResetPassword;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        public Task<AuthenticationResponseDTO> AuthenticationAsync(AuthenticationRequestDTO request);

        public Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request);

        public Task LogOutAsync();

        public Task<ConfirmAccountResponseDTO> ConfirmAccountAsync(ConfirmAccountRequestDTO request);

        public Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request);

        public Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request);
    }
}
