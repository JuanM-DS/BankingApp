using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.ViewModels.User;

namespace BankingApp.Core.Application.Mappings
{
    public class GeneralApplicationProfiles : Profile
    {
        public GeneralApplicationProfiles()
        {
            CreateMap<ApplicationUserDTO, SaveUserViewModel>()
                .ForMember(des => des.File, obj => obj.Ignore())
                .ReverseMap();

            CreateMap<ApplicationUserDTO, UserViewModel>()
                .ReverseMap();
        }
    }
}
