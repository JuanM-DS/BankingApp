using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Infrastructure.Identity.Entities;

namespace BankingApp.Infrastructure.Identity.Mappings
{
    internal class GeneralAccountProfiles : Profile
    {
        public GeneralAccountProfiles()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(des => des.Roles, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(des => des.Id, opt => opt.Ignore());
        }
    }
}
