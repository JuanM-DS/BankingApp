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
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper, ISavingsAccountService savingsAccountService, IPaymentRepository paymentRepository) : base(loanRepository, mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _savingsAccountService = savingsAccountService;
            _paymentRepository = paymentRepository;
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
