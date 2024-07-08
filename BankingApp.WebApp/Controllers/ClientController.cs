using BankingApp.Core.Application.CostomEntities;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;

        public ClientController(IBeneficiaryService beneficiaryService,ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService,IPaymentService paymentService, IUserService userService)
        {
            _beneficiaryService = beneficiaryService;
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _paymentService = paymentService;
            _userService = userService;
        }

        public async Task <IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> Beneficiary()
        {
            List<BeneficiaryViewModel> Vm = await _beneficiaryService.BeneficiariesList();
            return View("Beneficiary",Vm);
        }

        public async Task<IActionResult> CreateBeneficiary(int accountNumber)
        {
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(accountNumber);

            if (savingsAccount == null)
            {
                ViewBag.Message = "El numero de cuenta no existe";
                return View("Beneficiary");
            }
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = accountNumber;
            await _beneficiaryService.Add(vm);

            return View("Beneficiary");
        }

        public async Task<IActionResult> DeleteBeneficiary( int AccountNumber)
        {
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = AccountNumber;
            return View("DeleteBeneficiary", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBeneficiaryPost(SaveBeneficiaryViewModel vm)
        {
            await _beneficiaryService.Delete(vm.AccountNumber);
            return RedirectToRoute(new { controller = "Client", action = "Beneficiary" });
        }

        public async Task<IActionResult> ExpressPayment()
        {
            SaveExpressPaymentViewModel vm = new();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel();
            return View("ExpressPayment",vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(SaveExpressPaymentViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View("ExpressPayment", vm);
            }
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (ToAccount == null)
            {
                ViewBag.Message = "El número de cuenta no existe, se ha cancelado la transacción.";
                return View();
            }
            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View();
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View();
            }
            return View("ConfirmTransactionExpress", vm);
        }

        public async Task<IActionResult> ConfirmTransactionExpress(SaveExpressPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            Response<UserViewModel> user = await _userService.GetByNameAsync(savingsAccount.UserName);
            vm.FirstName = user.Data.FirstName;
            vm.LastName = user.Data.LastName;

            return View("ConfirmTransactionExpress", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTransactionExpressPost(SaveExpressPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            ToAccount.Balance += vm.Amount;
            await _savingsAccountService.Update(ToAccount,ToAccount.Id);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);
            FromAccount.Balance -= vm.Amount;
            await _savingsAccountService.Update(FromAccount,FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToAccountId;
            payment.Type = ((byte)PaymentTypes.Transfers);

            await _paymentService.Add(payment);

            return View("Index");
        }

        public async Task<IActionResult> CreditCardPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreditCardPayment(SaveCreditCardPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("CreditCardPayment", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View();
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View();
            }
           

            SaveCreditCardViewModel ToCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.ToCreditCardId);
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

            await _paymentService.Add(payment);

            return View("Index", vm);
        }


        public async Task<IActionResult> LoanRepayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoanRepayment(SaveLoanPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("LoanRepayment", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View();
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View();
            }
           

            SaveLoanViewModel ToLoan = await _loanService.GetByIdSaveViewModel(vm.ToLoanId);
            if(vm.Amount >= ToLoan.Balance)
            {
                vm.Amount = ToLoan.Balance;
                FromAccount.Balance -= vm.Amount;
                ToLoan.Balance -= vm.Amount;
            }
            FromAccount.Balance -= vm.Amount;
            ToLoan.Balance -= vm.Amount;

            await _loanService.Update(ToLoan, ToLoan.Id);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);

            SavePaymentViewModel payment = new();
            payment.Amount = vm.Amount;
            payment.FromProductId = vm.FromAccountId;
            payment.ToProductId = vm.ToLoanId;
            payment.Type = ((byte)PaymentTypes.PaymentToLoan);

            await _paymentService.Add(payment);

            return View("Index", vm);
        }


        public async Task<IActionResult> PaymentToBeneficiaries()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentToBeneficiaries(SavePaymentToBeneficiariesViewModel vm)
        {
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("PaymentToBeneficiaries", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View();
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View();
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
            payment.Type = ((byte)PaymentTypes.Transfers);

            await _paymentService.Add(payment);

            return View("Index", vm);
        }

        public async Task<IActionResult> CashAdvances()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CashAdvances(SaveCashAdvancesViewModel vm)
        {
            SaveCreditCardViewModel FromCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.FromCreditCardId);
            if (!ModelState.IsValid)
            {
                return View("CreditCardPayment", vm);
            }

            if (FromCreditCard.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta tarjeta de credito.";
                return View();
            }
            if (FromCreditCard.CreditLimit < vm.Amount)
            {
                ViewBag.InsufficientMoney = "Ese monto es mayor al limite de la tarjeta de credito, agrega uno menor";
                return View("CashAdvances");
            }

            if (FromCreditCard.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esta tarjeta de credito el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View("CashAdvances");
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

            await _paymentService.Add(payment);

            return View("Index", vm);
        }


        public async Task<IActionResult> TransferBetweenAccounts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransferBetweenAccounts(SaveTransferBetweenAccountsViewModel vm)
        {
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("PaymentToBeneficiaries", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View();
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.InsufficientMoney = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View();
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
            payment.Type = ((byte)PaymentTypes.Transfers);

            await _paymentService.Add(payment);

            return View("Index", vm);
        }

    }


}

