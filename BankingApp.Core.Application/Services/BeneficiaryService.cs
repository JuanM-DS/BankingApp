using AutoMapper;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace BankingApp.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IMapper mapper, IUserRepository userRepository) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<BeneficiaryViewModel>> BeneficiariesList()
        {
            List<BeneficiaryViewModel> beneficiaries =  GetAllViewModel().ToList();
            BeneficiaryViewModel beneficiary = new();
            List<BeneficiaryViewModel> fullBeneficiaries = new();

            foreach (var bn in beneficiaries)
            {
                var user = await _userRepository.Get()
                                              .FirstOrDefaultAsync(x => x.UserName == bn.UserName);
                beneficiary = bn;
                beneficiary = _mapper.Map<BeneficiaryViewModel>(user);
                fullBeneficiaries.Add(beneficiary);
            }

            return fullBeneficiaries;
        }

        public override async Task Delete(int id)
        {
            Beneficiary beneficiary = await _beneficiaryRepository.GetBeneficiary(id);
            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }
    }
}
