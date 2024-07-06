using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.DTOs.Account.ConfirmAccount;
using BankingApp.Core.Application.DTOs.Account.ForgotPassword;
using BankingApp.Core.Application.DTOs.Account.Register;
using BankingApp.Core.Application.DTOs.Account.ResetPassword;
using BankingApp.Core.Application.DTOs.User;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<AuthenticationResponseDTO> AuthenticationAsync(AuthenticationRequestDTO request);

        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request);

        Task LogOutAsync();

        Task<ConfirmAccountResponseDTO> ConfirmAccountAsync(ConfirmAccountRequestDTO request);

        Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request);

        Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request);
        Task<ApplicationUserDTO> GetUserByUserName(string userName);
    }
}
