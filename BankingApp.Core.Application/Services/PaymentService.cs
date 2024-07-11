using AutoMapper;
using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
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
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IBeneficiaryService _beneficiaryService;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IUserRepository userRepository, ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService, IBeneficiaryService beneficiaryService) : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _beneficiaryService = beneficiaryService;
        }

        public async Task<double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber)
        {
            return await _paymentRepository.TransactionsTillCutoffDay(CutoffDay, CreditCardNumber);
        }

        public async Task<List<PaymentViewModel>> GetAllViewModel(PaymentQueryFilters? filters = null)
        {
            var payments = _paymentRepository.GetAllWithInclude(x=>x.FromProduct, x=>x.ToProduct);
            if (filters is not null)
            {
                if (filters.PaymentTypes is not null && filters.PaymentTypes.Count > 1)
                {
                    payments = payments.Where(x => x.Type == (byte)PaymentTypes.PaymentToLoan || x.Type == (byte)PaymentTypes.PaymentToCreditCard);
                }
                else
                {
                    payments = payments.Where(x => x.Type == (byte)PaymentTypes.Transfer);
                }

                if (filters.Time is not null)
                    payments = payments.Where(x => x.CreatedTime.Day == filters.Time.Value.Day);

                if (filters.FromProductId is not null)
                    payments = payments.Where(x => x.FromProductId == filters.FromProductId);

                if (filters.ToProductId is not null)
                    payments = payments.Where(x => x.ToProductId == filters.ToProductId);
            }
            var paymentViewModels = _mapper.Map<List<PaymentViewModel>>(payments.AsEnumerable());

            return await SetUserViewModelToPayments(paymentViewModels);
        }

        public async Task<List<PaymentViewModel>> GetAllTransactions(List<PaymentTypes> paymentTypes)
        {
            var payments =  _paymentRepository.GetAllWithInclude(x => x.FromProduct, x => x.ToProduct);

            var transactionsId = paymentTypes.Select(x => (int)x).ToList();

            var transactions = payments.Where(x => transactionsId.Contains(x.Type));

            var transactionsViewModel = _mapper.Map<List<PaymentViewModel>>(transactions);

            return await SetUserViewModelToPayments(transactionsViewModel);
        }

        public async Task<List<PaymentViewModel>> GetAllTransfersOfToday(DateTime time)
        {
            var payments = _paymentRepository.GetAllWithInclude(x => x.FromProduct, x => x.ToProduct);

            var transactionsId = new List<int>() { (int)PaymentTypes.Deposit, (int)PaymentTypes.CashAdvance, (int)PaymentTypes.Transfer, (int)PaymentTypes.Disbursement };

            var transactions = payments.Where(x => transactionsId.Contains(x.Type));

            var transactionsOfToday = transactions.Where(x => x.CreatedTime == time);

            var transactionsOfTodayViewModel = _mapper.Map<List<PaymentViewModel>>(transactionsOfToday);

            return await SetUserViewModelToPayments(transactionsOfTodayViewModel);
        }

        public async Task<List<PaymentViewModel>> GetAllWithInclude()
        {
            var payments =  _paymentRepository.GetAllWithInclude(x => x.FromProduct, x => x.ToProduct);

            var paymentsViewModel = _mapper.Map<List<PaymentViewModel>>(payments);

            return await SetUserViewModelToPayments(paymentsViewModel);
        }

        public async Task<PaymentViewModel> GetByIdWithInclude(Guid id)
        {
            var payments =  _paymentRepository.GetAllWithInclude(x => x.FromProduct, x => x.ToProduct);

            var paymentViewModel = _mapper.Map<PaymentViewModel>(payments.FirstOrDefault(x => x.Id == id));

            return await SetUserViewModelToPayment(paymentViewModel);
        }

        // se encarga de cargar los usuarios en los paymentViewModel
        private async Task<List<PaymentViewModel>> SetUserViewModelToPayments(List<PaymentViewModel> source)
        {
            var users = await _userRepository.GetAsync(RoleTypes.Client);
            foreach (var item in source)
            {
                var userDto = users.FirstOrDefault(u => u.UserName == item.UserName);
                item.User = _mapper.Map<UserViewModel>(userDto);
            };

            return source;
        }

        private async Task<PaymentViewModel> SetUserViewModelToPayment(PaymentViewModel source)
        {
            var users = await _userRepository.GetAsync(RoleTypes.Client);

            var userDto = users.FirstOrDefault(u => u.UserName == source.UserName);
            source.User = _mapper.Map<UserViewModel>(userDto);
            return source;
        }

        public async Task<Response<SaveExpressPaymentViewModel>> ExpressPayment(SaveExpressPaymentViewModel vm, string UserName)
        {
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();

            
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (ToAccount == null)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "El número de cuenta de destino no existe.",
                    Success = false
                };
            }
            if (FromAccount.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "No tienes dinero en esta cuenta.",
                    Success = false
                };
            }

            if (FromAccount.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor o igual a RD$" + FromAccount.Balance,
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }
            if (vm.ToAccountId == vm.FromAccountId)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "No se puede realizar una transaccion entre la misma cuenta.",
                    Success = false
                };
            }
            if (ToAccount.UserName == UserName)
            {
                return new()
                {
                    Data = vm,
                    View = "ExpressPayment",
                    Error = "Transacion cancelada. Para hacer transaciones entre cuentas propias vaya a \"Tranferencia entre mis cuentas\".",
                    Success = false
                };
            }
            return new()
            {
                Data = vm,
                View = "ConfirmTransactionExpress",
                Success = true
            };

        }

        public async Task ConfirmTransactionExpressPost(SaveExpressPaymentViewModel vm, string UserName)
        {
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            ToAccount.Balance += vm.Amount;
            await _savingsAccountService.Update(ToAccount, ToAccount.Id);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);
            FromAccount.Balance -= vm.Amount;
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToAccountId;
            payment.Type = ((byte)PaymentTypes.Transfer);
            payment.UserName = UserName;

            await base.Add(payment);
        }

        public async Task<Response<SaveCreditCardPaymentViewModel>> CreditCardPayment(SaveCreditCardPaymentViewModel vm, string UserName)
        {
            vm.ToCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);
            SaveCreditCardViewModel ToCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.ToCreditCardId);

            if (FromAccount.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "CreditCardPayment",
                    Error = "No tienes dinero en esta cuenta.",
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "CreditCardPayment",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }

            if (FromAccount.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "CreditCardPayment",
                    Error = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor o igual a RD$" + FromAccount.Balance,
                    Success = false
                };
            }
            if (ToCreditCard.CreditLimit == ToCreditCard.Balance)
            {
                return new()
                {
                    Data = vm,
                    View = "CreditCardPayment",
                    Error = "Ya pagaste todo lo que debias de esa tarjeta de credito, selecciona otra o vuelve atras.",
                    Success = false
                };
            }


            double transferAmount = Math.Min(vm.Amount, ToCreditCard.CreditLimit - ToCreditCard.Balance);

            ToCreditCard.Balance += transferAmount;
            FromAccount.Balance -= transferAmount;

            await _creditCardService.Update(ToCreditCard, ToCreditCard.Id);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToCreditCardId;
            payment.Type = ((byte)PaymentTypes.PaymentToCreditCard);
            payment.UserName = UserName;

            await base.Add(payment);

            return new()
            {
                View = "Index",
                Success = true
            };
        }

        public async Task<Response<SaveLoanPaymentViewModel>> LoanPayment(SaveLoanPaymentViewModel vm, string UserName)
        {
            vm.ToLoans = _loanService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (FromAccount.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "LoanPayment",
                    Error = "No tienes dinero en esta cuenta.",
                    Success = false
                };
            }

            if (FromAccount.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "LoanPayment",
                    Error = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$" + FromAccount.Balance,
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "LoanPayment",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }


            SaveLoanViewModel ToLoan = await _loanService.GetByIdSaveViewModel(vm.ToLoanId);
            if (vm.Amount >= ToLoan.Balance)
            {
                vm.Amount = ToLoan.Balance;
                FromAccount.Balance -= vm.Amount;
                ToLoan.Balance -= vm.Amount;
            }
            else
            {
                FromAccount.Balance -= vm.Amount;
                ToLoan.Balance -= vm.Amount;
            }
            

            await _loanService.Update(ToLoan, ToLoan.Id);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToLoanId;
            payment.Type = ((byte)PaymentTypes.PaymentToLoan);
            payment.UserName = UserName;

            await base.Add(payment);

            return new()
            {
                View = "Index",
                Success = true
            };
        }

        public async Task<Response<SavePaymentToBeneficiariesViewModel>> PaymentToBeneficiaries(SavePaymentToBeneficiariesViewModel vm, string UserName)
        {
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();
            vm.ToBeneficiaries = _beneficiaryService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (FromAccount.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "PaymentToBeneficiaries",
                    Error = "No tienes dinero en esta cuenta.",
                    Success = false
                };
            }

            if (FromAccount.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "PaymentToBeneficiaries",
                    Error = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor o igual a RD$" + FromAccount.Balance,
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "PaymentToBeneficiaries",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }


            SaveSavingsAccountViewModel Tobeneficiary = await _savingsAccountService.GetByIdSaveViewModel(vm.ToBeneficiaryId);
            if (vm.Amount >= Tobeneficiary.Balance)
            {
                vm.Amount = Tobeneficiary.Balance;
                FromAccount.Balance -= vm.Amount;
                Tobeneficiary.Balance += vm.Amount;
            }
            FromAccount.Balance -= vm.Amount;
            Tobeneficiary.Balance += vm.Amount;

            await _savingsAccountService.Update(Tobeneficiary, vm.ToBeneficiaryId);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToBeneficiaryId;
            payment.Type = ((byte)PaymentTypes.Transfer);
            payment.UserName = UserName;

            await base.Add(payment);

            return new()
            {
                View = "Index",
                Success = true
            };
        }


        public async Task<Response<SaveCashAdvancesViewModel>> CashAdvances(SaveCashAdvancesViewModel vm, string UserName)
        {
            vm.FromCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();
            vm.ToAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();

            SaveCreditCardViewModel FromCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.FromCreditCardId);
            if (FromCreditCard.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "CashAdvances",
                    Error = "No tienes dinero en esta tarjeta de credito.",
                    Success = false
                };
            }
            if (FromCreditCard.CreditLimit < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "CashAdvances",
                    Error = "Ese monto es mayor al limite de la tarjeta de credito, agrega uno menor al limite de RD$" + FromCreditCard.CreditLimit,
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "CashAdvances",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }

            if (FromCreditCard.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "CashAdvances",
                    Error = "No tienes en esta tarjeta de credito el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$" + FromCreditCard.Balance,
                    Success = false
                };
            }

            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            FromCreditCard.Balance -= vm.Amount * 1.0625;
            ToAccount.Balance += vm.Amount;

            await _creditCardService.Update(FromCreditCard, FromCreditCard.Id);
            await _savingsAccountService.Update(ToAccount, ToAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromCreditCardId;
            payment.ToProductId = vm.ToAccountId;
            payment.Type = ((byte)PaymentTypes.CashAdvance);
            payment.UserName = UserName;

            await base.Add(payment);

            return new()
            {
                View = "Index",
                Success = true
            };
        }

        public async Task<Response<SaveTransferBetweenAccountsViewModel>> TransferBetweenAccounts(SaveTransferBetweenAccountsViewModel vm, string UserName)
        {
            vm.Accounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == UserName).ToList();
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (FromAccount.Balance == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "TransferBetweenAccounts",
                    Error = "No tienes dinero en esta cuenta.",
                    Success = false
                };
            }
            if (vm.FromAccountId == vm.ToAccountId)
            {
                return new()
                {
                    Data = vm,
                    View = "TransferBetweenAccounts",
                    Error = "No se puede realizar una transaccion entre la misma cuenta.",
                    Success = false
                };
            }
            if (vm.Amount == 0)
            {
                return new()
                {
                    Data = vm,
                    View = "TransferBetweenAccounts",
                    Error = "El monto no puede ser cero, agrega uno mayor.",
                    Success = false
                };
            }

            if (FromAccount.Balance < vm.Amount)
            {
                return new()
                {
                    Data = vm,
                    View = "TransferBetweenAccounts",
                    Error = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$" + FromAccount.Balance,
                    Success = false
                };
            }


            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            if (vm.Amount >= ToAccount.Balance)
            {
                vm.Amount = ToAccount.Balance;
                FromAccount.Balance -= vm.Amount;
                ToAccount.Balance += vm.Amount;
            }
            FromAccount.Balance -= vm.Amount;
            ToAccount.Balance += vm.Amount;

            await _savingsAccountService.Update(ToAccount, vm.ToAccountId);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToAccountId;
            payment.Type = ((byte)PaymentTypes.Transfer);
            payment.UserName = UserName;
            await base.Add(payment);

            return new()
            {
                View = "Index",
                Success = true
            };
        }
    }
}
