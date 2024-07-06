using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Domain.Entities;
using System.Collections.Generic;

namespace BankingApp.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IMapper _mapper;
        private readonly IAccountServices _accountServices;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IMapper mapper, IAccountServices accountServices) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _mapper = mapper;
            _accountServices = accountServices;
        }

        public async Task<List<BeneficiaryViewModel>> ListBeneficiaries()
        {
            List<BeneficiaryViewModel> beneficiaries = await GetAllViewModel();
            BeneficiaryViewModel beneficiary = new();
            List<BeneficiaryViewModel> fullBeneficiaries = new();
            ApplicationUserDTO user = new();

            foreach (var bn in beneficiaries)
            {
                user = await _accountServices.GetUserByUserName(bn.UserUserName);
                beneficiary = bn;
                beneficiary = _mapper.Map<BeneficiaryViewModel>(user);
                fullBeneficiaries.Add(beneficiary);
            }

            return fullBeneficiaries;
        }
    }
}
