using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class LoanService : GenericService<SaveLoanViewModel, LoanViewModel, Loan>, ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper) : base(loanRepository, mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public List<LoanViewModel> GetAllByUserWithInclude(string userName)
        {
            var loans = _loanRepository.GetAllWithInclude(x => x.PaymentsFrom, x => x.PaymentsTo);

            var loansByUser = loans.Where(x => x.UserName == userName)
                                               .ToList();

            var loansViewModels = _mapper.Map<List<LoanViewModel>>(loansByUser);

            return loansViewModels;
        }
    }
}
