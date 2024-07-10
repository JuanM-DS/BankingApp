using AutoMapper;
using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.DTOs.User;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.User;
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
        private readonly ISavingsAccountService _savingsAccountService;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IMapper mapper, IUserRepository userRepository, ISavingsAccountService savingsAccountService) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _savingsAccountService = savingsAccountService;
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

        public async Task DeleteBeneficiary(string userName, int accountNumber)
        {
            Beneficiary beneficiary = await _beneficiaryRepository.GetBeneficiary(userName, accountNumber);
            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }

        public async Task<Response<List<BeneficiaryViewModel>>> CreateBeneficiary(int accountNumber, string UserName)
        {
            List<BeneficiaryViewModel> VmList = new();
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(accountNumber);
            var beneficiary = GetAllViewModel().Where(b => b.AccountNumber == accountNumber && b.UserName == UserName).ToList();

            if (savingsAccount == null)
            {
                VmList = BeneficiariesList().Where(b => b.UserName == UserName).ToList();
                return new()
                {
                    Data = VmList,
                    View = "Beneficiary",
                    Error = "El numero de cuenta no existe",
                    Success = false
                };
            }
            if (savingsAccount.UserName == UserName)
            {
                VmList = BeneficiariesList().Where(b => b.UserName == UserName).ToList();
                return new()
                {
                    Data = VmList,
                    View = "Beneficiary",
                    Error = "No puedes agregar un numero de cuenta propio como beneficiario.",
                    Success = false
                };
            }
            if (beneficiary.Count != 0)
            {
                VmList = BeneficiariesList().Where(b => b.UserName == UserName).ToList();
                return new()
                {
                    Data = VmList,
                    View = "Beneficiary",
                    Error = "Ya tienes ese beneficiario agregado",
                    Success = false
                };
            }
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = accountNumber;
            vm.UserName = UserName;
            await Add(vm);

            return new()
            {
                Data = VmList,
                Success = true,
                View = "Beneficiary",
            };
        }
    }
}
