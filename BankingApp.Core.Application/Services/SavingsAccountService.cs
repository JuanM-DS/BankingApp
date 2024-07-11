using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IMapper mapper, IProductRepository productRepository) : base(savingsAccountRepository, mapper)
        {
            _savingsAccountRepository = savingsAccountRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public override async Task Add(SaveSavingsAccountViewModel savesavingsAccountViewModel)
        {
            var savingsAccount = _mapper.Map<SavingsAccount>(savesavingsAccountViewModel);
            
            savingsAccount = await _savingsAccountRepository.AddAsync(savingsAccount);
            Product product = new();
            product.Id = savingsAccount.Id;
            product.UserName = savingsAccount.UserName;
            product.CreatedTime = savingsAccount.CreatedTime;
            product.CreatedBy = savingsAccount.CreatedBy;

            await _productRepository.AddAsync(product);
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
