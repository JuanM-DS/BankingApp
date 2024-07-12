using AutoMapper;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class LoanService : GenericService<SaveLoanViewModel, LoanViewModel, Loan>, ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper, IProductRepository productRepository, ISavingsAccountService savingsAccountService, IPaymentRepository paymentRepository) : base(loanRepository, mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _savingsAccountService = savingsAccountService;
            _paymentRepository = paymentRepository;
        }

        public override async Task Add(SaveLoanViewModel saveLoanViewModel)
        {
            var loan = _mapper.Map<Loan>(saveLoanViewModel);

            loan = await _loanRepository.AddAsync(loan);
            Product product = new();
            product.Id = loan.Id;
            product.UserName = loan.UserName;
            product.CreatedTime = loan.CreatedTime;
            product.CreatedBy = loan.CreatedBy;
            product.Type = (byte)ProductTypes.Loan;
            await _productRepository.AddAsync(product);

            Payment payment = new();
            payment.Amount = loan.Principal;
            payment.FromProductId = loan.Id;
            payment.ToProductId = (await _savingsAccountService.GetPrincipalAccount(loan.UserName)).Id;
            payment.UserName = loan.UserName;
            payment.Type = (byte)PaymentTypes.Disbursement;

            await _paymentRepository.AddAsync(payment);

            await _savingsAccountService.TransferFromLoan(loan.Principal, (int)payment.ToProductId);

        }
        public List<LoanViewModel> GetAllByUserWithInclude(string userName)
        {
            var loans = _loanRepository.GetAllWithInclude();

            var loansByUser = loans.Where(x => x.UserName == userName)
                                               .ToList();

            var loansViewModels = _mapper.Map<List<LoanViewModel>>(loansByUser);

            return loansViewModels;
        }
    }
}
