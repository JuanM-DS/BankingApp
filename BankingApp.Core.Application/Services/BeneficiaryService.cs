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

        public List<BeneficiaryViewModel> BeneficiariesList()
        {
            List<BeneficiaryViewModel> beneficiaries =  GetAllViewModel().ToList();
            BeneficiaryViewModel beneficiary = new();
            List<BeneficiaryViewModel> fullBeneficiaries = new();

            foreach (var bn in beneficiaries)
            {
                var user = _userRepository.Get()
                                              .FirstOrDefault(x => x.UserName == bn.UserName);
                beneficiary = bn;
                beneficiary.FirstName = user.FirstName;
                beneficiary.LastName = user.LastName;
                fullBeneficiaries.Add(beneficiary);
            }

            return fullBeneficiaries;
        }

        public async Task Delete(string userName, int accuntNumber)
        {
            Beneficiary beneficiary = await _beneficiaryRepository.GetBeneficiary(userName, accuntNumber);
            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }
    }
}
