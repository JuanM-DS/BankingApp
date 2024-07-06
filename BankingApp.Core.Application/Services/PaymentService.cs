using AutoMapper;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel, PaymentViewModel, Payment>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IUserRepository userRepository) : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber)
        {
            return await _paymentRepository.TransactionsTillCutoffDay(CutoffDay, CreditCardNumber);
        }

        public async Task<List<PaymentViewModel>> GetAllViewModel(PaymentQueryFilters? filters = null)
        {
            var payments = await _paymentRepository.GetAllWithIncludeAsync(["FromAccount", "FromCrediCard", "FromLoan", "ToAccount", "ToCreditCard", "ToLoan"]);

            if(filters is not null)
            {
                if (filters.ProductTypes is not null)
                    payments = payments.Where(x => x.ProductType == (int)filters.ProductTypes).ToList();

                if (filters.PaymentTypes is not null)
                    payments = payments.Where(x => x.Type == (int)filters.PaymentTypes).ToList();

                if (filters.Time is not null)
                    payments = payments.Where(x => x.CreatedTime == filters.Time).ToList();

                if (filters.FromProductId is not null)
                    payments = payments.Where(x => x.FromProductId == filters.FromProductId).ToList();

                if (filters.ToProductId is not null)
                    payments = payments.Where(x => x.ToProductId == filters.ToProductId).ToList();
            }
            var paymentViewModels = _mapper.Map<List<PaymentViewModel>>(payments);

            return await SetUserViewModelToPayments(paymentViewModels);
        }

        public async Task<List<PaymentViewModel>> GetAllTransactions()
        {
            var payments = await _paymentRepository.GetAllWithIncludeAsync(["FromAccount", "FromCrediCard", "FromLoan", "ToAccount", "ToCreditCard", "ToLoan"]);

            var transactionsId = new List<int>() { (int)PaymentTypes.InitialDeposit, (int)PaymentTypes.CashAdvance, (int)PaymentTypes.Transfers, (int)PaymentTypes.Disbursement};

            var transactions = payments.Where(x => transactionsId.Contains(x.Type));

            var transactionsViewModel = _mapper.Map<List<PaymentViewModel>>(transactions);

            return await SetUserViewModelToPayments(transactionsViewModel);
        }

        public async Task<List<PaymentViewModel>> GetAllTransfersOfToday(DateTime time)
        {
            var payments = await _paymentRepository.GetAllWithIncludeAsync(["FromAccount", "FromCrediCard", "FromLoan", "ToAccount", "ToCreditCard", "ToLoan"]);

            var transactionsId = new List<int>() { (int)PaymentTypes.InitialDeposit, (int)PaymentTypes.CashAdvance, (int)PaymentTypes.Transfers, (int)PaymentTypes.Disbursement };

            var transactions = payments.Where(x => transactionsId.Contains(x.Type));

            var transactionsOfToday = transactions.Where(x => x.CreatedTime == time);

            var transactionsOfTodayViewModel = _mapper.Map<List<PaymentViewModel>>(transactionsOfToday);

            return await SetUserViewModelToPayments(transactionsOfTodayViewModel);
        }

        public async Task<List<PaymentViewModel>> GetAllWithInclude()
        {
            var payments = await _paymentRepository.GetAllWithIncludeAsync(["FromAccount", "FromCrediCard", "FromLoan", "ToAccount", "ToCreditCard", "ToLoan"]);

            var paymentsViewModel = _mapper.Map<List<PaymentViewModel>>(payments);

            return await SetUserViewModelToPayments(paymentsViewModel);
        }

        public async Task<PaymentViewModel> GetAllByIdWithInclude(int id)
        {
            var payments = await _paymentRepository.GetAllWithIncludeAsync(["FromAccount", "FromCrediCard", "FromLoan", "ToAccount", "ToCreditCard", "ToLoan"]);

            var paymentViewModel = _mapper.Map<PaymentViewModel>(payments.FirstOrDefault(x => x.Id == id));

            return await SetUserViewModelToPayment(paymentViewModel);
        }

        // se encarga de cargar los usuarios en los paymentViewModel
        private async Task<List<PaymentViewModel>> SetUserViewModelToPayments(List<PaymentViewModel> source)
        {
            var query = await _userRepository.GetAsync(RoleTypes.Client);
            foreach (var item in source)
            {
                var userDto = await query.FirstOrDefaultAsync(u => u.UserName == item.UserName);
                item.User = _mapper.Map<UserViewModel>(userDto);
            };

            return source;
        }

        private async Task<PaymentViewModel> SetUserViewModelToPayment(PaymentViewModel source)
        {
            var query = await _userRepository.GetAsync(RoleTypes.Client);

            var userDto = await query.FirstOrDefaultAsync(u => u.UserName == source.UserName);
            source.User = _mapper.Map<UserViewModel>(userDto);
            return source;
        }
    }
}
