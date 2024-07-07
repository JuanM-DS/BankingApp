using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        private readonly IMapper _mapper;

        public SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IMapper mapper) : base(savingsAccountRepository, mapper)
        {
            _savingsAccountRepository = savingsAccountRepository;
            _mapper = mapper;
        }

        public List<SavingsAccount> GetAllByUserWithInclude(string userName)
        {
            var savingsAccount = _savingsAccountRepository.GetAllWithInclude(x => x.PaymentsFrom, x => x.PaymentsTo, x=>x.Beneficiaries);

            var savingsAccounts = savingsAccount.Where(x => x.UserName == userName)
                                               .ToList();

            var savingsAccountViewModel = _mapper.Map<List<SavingsAccount>>(savingsAccounts);

            return savingsAccountViewModel;
        }
    }
}
