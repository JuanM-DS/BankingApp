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
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IMapper mapper, IProductRepository productRepository, IPaymentRepository paymentRepository) : base(savingsAccountRepository, mapper)
        {
            _savingsAccountRepository = savingsAccountRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
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
            product.Type = (byte)ProductTypes.SavingsAccount;
            await _productRepository.AddAsync(product);

            if (savingsAccount.Balance > 0)
            {
                Payment payment = new();
                payment.Amount = (double)savingsAccount.Balance;
                payment.ToProductId = savingsAccount.Id;
                payment.UserName = savingsAccount.UserName;
                payment.Type = (byte)PaymentTypes.Deposit;

                await _paymentRepository.AddAsync(payment);

                savingsAccount.Balance = payment.Amount;
                await _savingsAccountRepository.UpdateAsync(savingsAccount, savingsAccount.Id);
            }
        }

        public override async Task Update(SaveSavingsAccountViewModel saveSavingsAccountViewModel, int id)
        {
            var savingsAccount = await _savingsAccountRepository.GetByIdAsync(id);

            if (saveSavingsAccountViewModel.Balance > 0)
            {
                Payment payment = new();
                payment.Amount = (double)saveSavingsAccountViewModel.Balance;
                payment.ToProductId = savingsAccount.Id;
                payment.UserName = savingsAccount.UserName;
                payment.Type = (byte)PaymentTypes.Deposit;

                await _paymentRepository.AddAsync(payment);

                savingsAccount.Balance += payment.Amount;
                await _savingsAccountRepository.UpdateAsync(savingsAccount, savingsAccount.Id);
            }
        }

        public async Task Delete(int accountToDeleteId, int principalAccountId)
        {
            var savingsAccount = await _savingsAccountRepository.GetByIdAsync(accountToDeleteId);
            var principalAccount = await _savingsAccountRepository.GetByIdAsync(principalAccountId);

            if (savingsAccount.Balance > 0)
            {
                Payment payment = new();
                payment.Amount = (double)savingsAccount.Balance;
                payment.FromProductId = savingsAccount.Id;
                payment.ToProductId = principalAccount.Id;
                payment.UserName = savingsAccount.UserName;
                payment.Type = (byte)PaymentTypes.Transfer;

                await _paymentRepository.AddAsync(payment);

                principalAccount.Balance += payment.Amount;
                await _savingsAccountRepository.UpdateAsync(principalAccount, principalAccount.Id);
            }

            await _productRepository.DeleteAsync(savingsAccount.Id);
            await _savingsAccountRepository.DeleteAsync(savingsAccount);
            
        }
        public async Task TransferFromLoan(double amount, int id)
        {
            var savingsAccount = await _savingsAccountRepository.GetByIdAsync(id);
            savingsAccount.Balance += amount;

            await _savingsAccountRepository.UpdateAsync(savingsAccount, savingsAccount.Id);
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
