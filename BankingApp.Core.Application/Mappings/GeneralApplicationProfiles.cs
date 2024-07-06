using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.User;
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
               .ForMember(des => des.UserUserName, opt => opt.Ignore());

            CreateMap<ApplicationUserDTO, SaveUserViewModel>()
                .ForMember(des => des.File, obj => obj.Ignore())
                .ReverseMap();

            CreateMap<ApplicationUserDTO, UserViewModel>()
                .ReverseMap();
            #endregion

            #region payments
            CreateMap<Payment, SavePaymentViewModel>()
                .ReverseMap()
                .ForMember(des => des.FromAccount, opt => opt.Ignore())
                .ForMember(des => des.FromCrediCard, opt => opt.Ignore())
                .ForMember(des => des.FromLoan, opt => opt.Ignore())
                .ForMember(des => des.ToAccount, opt => opt.Ignore())
                .ForMember(des => des.ToCreditCard, opt => opt.Ignore())
                .ForMember(des => des.ToLoan, opt => opt.Ignore());

            CreateMap<Payment, PaymentViewModel>()
                .ReverseMap();
            #endregion

            #region nex region
            #endregion
        }
    }
}
