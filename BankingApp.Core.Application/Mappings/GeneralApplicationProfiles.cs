using AutoMapper;
using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.DTOs.Account.ConfirmAccount;
using BankingApp.Core.Application.DTOs.Account.ForgotPassword;
using BankingApp.Core.Application.DTOs.Account.ResetPassword;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.ViewModels.Account;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.Product;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Mappings
{
    public class GeneralApplicationProfiles : Profile
    {
        public GeneralApplicationProfiles()
        {
            #region UserProfile
            CreateMap<BeneficiaryViewModel, ApplicationUserDTO>()
               .ForMember(des => des.Id, opt => opt.Ignore())
               .ForMember(des => des.UserName, opt => opt.Ignore())
               .ForMember(des => des.Email, opt => opt.Ignore())
               .ForMember(des => des.EmailConfirmed, opt => opt.Ignore())
               .ForMember(des => des.IdCard, opt => opt.Ignore())
               .ForMember(des => des.Status, opt => opt.Ignore())
               .ForMember(des => des.Roles, opt => opt.Ignore())
               .ForMember(des => des.PhotoUrl, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(des => des.AccountNumber, opt => opt.Ignore())
               .ForMember(des => des.UserName, opt => opt.Ignore());

            CreateMap<ApplicationUserDTO, UserViewModel>()
               .ReverseMap()
               .ForMember(des => des.Password, opt => opt.Ignore());

            CreateMap<ApplicationUserDTO, SaveUserViewModel>()
                .ForMember(des => des.File, obj => obj.Ignore())
                .ForMember(des => des.Role, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(des => des.Roles, opt => opt.Ignore());

            CreateMap<ApplicationUserDTO, UserViewModel>()
                .ReverseMap();
            #endregion

            #region payments
            CreateMap<Payment, SavePaymentViewModel>()
                .ReverseMap()
                .ForMember(des => des.FromProduct, opt => opt.Ignore())
                .ForMember(des => des.ToProduct, opt => opt.Ignore());

            CreateMap<Payment, PaymentViewModel>()
                .ReverseMap();
            #endregion

            #region Account
            CreateMap<AuthenticationRequestDTO, LoginViewModel>()
                .ReverseMap();

            CreateMap<ForgotPasswordViewModel, ForgotPasswordRequestDTO>()
                .ReverseMap();

            CreateMap<ResetPasswordViewModel, ResetPasswordRequestDTO>()
                .ReverseMap();

            CreateMap<ConfirmAccountViewModel, ConfirmAccountRequestDTO>()
                .ReverseMap();
            #endregion

            #region Beneficiary
            CreateMap<Beneficiary, BeneficiaryViewModel>()
               .ForMember(des => des.FirstName, opt => opt.Ignore())
               .ForMember(des => des.LastName, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(des => des.AccountNumber, opt => opt.Ignore())
               .ForMember(des => des.SavingsAccount, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());

            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
               .ReverseMap()
               .ForMember(des => des.SavingsAccount, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());
            #endregion

            #region SavingsAccount
            CreateMap<SavingsAccount, SaveSavingsAccountViewModel>()
               .ReverseMap()
               .ForMember(des => des.Beneficiaries, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());

            CreateMap<SavingsAccount, SavingsAccountViewModel>()
               .ReverseMap()
               .ForMember(des => des.UserName, opt => opt.Ignore())
               .ForMember(des => des.Balance, opt => opt.Ignore())
               .ForMember(des => des.IsPrincipal, opt => opt.Ignore())
               .ForMember(des => des.Beneficiaries, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());
            #endregion

            #region Loans
            CreateMap<Loan, LoanViewModel>()
               .ReverseMap()
               .ForMember(des => des.Installment, opt => opt.Ignore())
               .ForMember(des => des.PaymentDay, opt => opt.Ignore())
               .ForMember(des => des.InterestRate, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());

            CreateMap<Loan, SaveLoanViewModel>()
               .ReverseMap()
               .ForMember(des => des.Installment, opt => opt.Ignore())
               .ForMember(des => des.PaymentDay, opt => opt.Ignore())
               .ForMember(des => des.InterestRate, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());
            #endregion

            #region CreditCard
            CreateMap<CreditCard, CreditCardViewModel>()
               .ReverseMap()
               .ForMember(des => des.CutoffDay, opt => opt.Ignore())
               .ForMember(des => des.PaymentDay, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());

            CreateMap<CreditCard, SaveCreditCardViewModel>()
               .ReverseMap()
               .ForMember(des => des.CutoffDay, opt => opt.Ignore())
               .ForMember(des => des.PaymentDay, opt => opt.Ignore())
               .ForMember(des => des.CreditLimit, opt => opt.Ignore())
               .ForMember(des => des.CreatedBy, opt => opt.Ignore())
               .ForMember(des => des.CreatedTime, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedBy, opt => opt.Ignore())
               .ForMember(des => des.LastModifiedTime, opt => opt.Ignore());
            #endregion

            #region Product

            CreateMap<SaveProductViewModel, Product>()
                .ReverseMap();

            CreateMap<ProductViewModel, Product>()
                .ReverseMap();

            CreateMap<SaveProductViewModel, ProductViewModel>()
                .ReverseMap();
            #endregion

            #region nex region
            #endregion
        }
    }
}
