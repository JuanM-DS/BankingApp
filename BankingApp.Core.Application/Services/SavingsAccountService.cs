using AutoMapper;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IMapper mapper, IPaymentRepository paymentRepository) : base(savingsAccountRepository, mapper)
        {
            _savingsAccountRepository = savingsAccountRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
        }
        public async Task<SaveSavingsAccountViewModel> GetPrincipalAccount(string userName)
        {
            return _mapper.Map<SaveSavingsAccountViewModel>(await _savingsAccountRepository.GetPrincipalAccountAsync(userName));
        }

        public List<SavingsAccount> GetAllByUserWithInclude(string userName)
        {
            var savingsAccount = _savingsAccountRepository.GetAllWithInclude();

            var savingsAccounts = savingsAccount.Where(x => x.UserName == userName)
                                               .ToList();

            var savingsAccountViewModel = _mapper.Map<List<SavingsAccount>>(savingsAccounts);

            return savingsAccountViewModel;
        }
    }
}
