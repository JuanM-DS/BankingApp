using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.DTOs.Account.Authentication;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.ViewModels.Product;
using BankingApp.Core.Application.ViewModels.Message;

namespace BankingApp.WebApp.Controllers
{
    public class ClientController : Controller
    {
        public readonly IBeneficiaryService _beneficiaryService;
        public readonly ISavingsAccountService _savingsAccountService;
        public readonly ICreditCardService _creditCardService;
        public readonly ILoanService _loanService;
        public readonly IPaymentService _paymentService;
        public readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public ClientController(IBeneficiaryService beneficiaryService,ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService,IPaymentService paymentService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _beneficiaryService = beneficiaryService;
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public  IActionResult Index(string message ="")
        {
            ViewBag.Message = message;
            ProductsViewModel products = new();
            products.Loans = _loanService.GetAllViewModel().Where(l => l.UserName == userViewModel.UserName).ToList();
            products.savingsAccounts =  _savingsAccountService.GetAllViewModel().Where(s => s.UserName == userViewModel.UserName).ToList();
            products.CreditCards = _creditCardService.GetAllViewModel()
                .Where(c => c.UserName == userViewModel.UserName)
                .Select(c => new CreditCardViewModel
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    CreditLimit = c.CreditLimit,
                    Balance = c.CreditLimit - c.Balance
                })
                .ToList();

            return View("Index",products);
        }

        public IActionResult Beneficiary(string message = "")
        {
            ViewBag.Message = message;
            List<BeneficiaryViewModel> Vm = _beneficiaryService.BeneficiariesList().Where(b => b.UserName == userViewModel.UserName).ToList();
            return View("Beneficiary",Vm);
        }

        public async Task<IActionResult> CreateBeneficiary(int accountNumber)
        {
            List<BeneficiaryViewModel> VmList = new();
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(accountNumber);
            var beneficiary =  _beneficiaryService.GetAllViewModel().Where(b => b.AccountNumber == accountNumber && b.UserName == userViewModel.UserName).ToList();

            if (savingsAccount == null)
            {
                ViewBag.Message = "El numero de cuenta no existe";
                VmList = _beneficiaryService.BeneficiariesList().Where(b => b.UserName == userViewModel.UserName).ToList();
                return View("Beneficiary", VmList);
            }
            if (savingsAccount.UserName == userViewModel.UserName)
            {
                ViewBag.Message = "No puedes agregar un numero de cuenta propio como beneficiario.";
                VmList = _beneficiaryService.BeneficiariesList().Where(b => b.UserName == userViewModel.UserName).ToList();
                return View("Beneficiary", VmList);
            }
            if (beneficiary.Count != 0)
            {
                ViewBag.Message = "Ya tienes ese beneficiario agregado";
                VmList = _beneficiaryService.BeneficiariesList().Where(b => b.UserName == userViewModel.UserName).ToList();
                return View("Beneficiary",VmList);
            }
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = accountNumber;
            vm.UserName = userViewModel.UserName;
            await _beneficiaryService.Add(vm);

            return RedirectToAction("Beneficiary");
        }

        public async Task<IActionResult> DeleteBeneficiary( int AccountNumber)
        {
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = AccountNumber;
            vm.UserName = userViewModel.UserName;
            return View("DeleteBeneficiary", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBeneficiaryPost(SaveBeneficiaryViewModel vm)
        {
            await _beneficiaryService.DeleteBeneficiary(vm.UserName,vm.AccountNumber);
            return RedirectToAction("Beneficiary");
        }

        public async Task<IActionResult> ExpressPayment()
        {
            SaveExpressPaymentViewModel vm = new();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("ExpressPayment",vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(SaveExpressPaymentViewModel vm)
        {
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();

            if (!ModelState.IsValid)
            {
                return View("ExpressPayment", vm);
            }
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (ToAccount == null)
            {
                ViewBag.Message = "El número de cuenta de destino no existe.";
                return View("ExpressPayment",vm);
            }
            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View("ExpressPayment",vm);
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor o igual a RD$"+FromAccount.Balance;
                return View("ExpressPayment", vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("ExpressPayment", vm);
            }

            return RedirectToAction("ConfirmTransactionExpress", vm);
        }

        public async Task<IActionResult> ConfirmTransactionExpress(SaveExpressPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            Response<UserViewModel> user = _userService.GetByNameAsync(savingsAccount.UserName);
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
            payment.Type = ((byte)PaymentTypes.Transfer);
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }

        public  IActionResult CreditCardPayment()
        {
            SaveCreditCardPaymentViewModel vm = new();
            vm.ToCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("CreditCardPayment", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreditCardPayment(SaveCreditCardPaymentViewModel vm)
        {
            vm.ToCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("CreditCardPayment", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View("CreditCardPayment", vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("CreditCardPayment", vm);
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor o igual a RD$"+FromAccount.Balance;
                return View("CreditCardPayment", vm);
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
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }


        public async Task<IActionResult> LoanPayment()
        {
            SaveLoanPaymentViewModel vm = new();
            vm.ToLoans = _loanService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("LoanPayment", vm);
        }

        [HttpPost]
        public async Task<IActionResult> LoanPayment(SaveLoanPaymentViewModel vm)
        {
            vm.ToLoans = _loanService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("LoanPayment", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View("LoanPayment", vm);
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$"+FromAccount.Balance;
                return View("LoanPayment", vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("LoanPayment", vm);
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
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }


        public async Task<IActionResult> PaymentToBeneficiaries()
        {
            SavePaymentToBeneficiariesViewModel vm = new();
            vm.FromAccounts =_savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.ToBeneficiaries = _beneficiaryService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("PaymentToBeneficiaries",vm);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentToBeneficiaries(SavePaymentToBeneficiariesViewModel vm)
        {
            vm.FromAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.ToBeneficiaries = _beneficiaryService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();

            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("PaymentToBeneficiaries", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View("PaymentToBeneficiaries",vm);
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto menor";
                return View("PaymentToBeneficiaries",vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("PaymentToBeneficiaries", vm);
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
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }

        public async Task<IActionResult> CashAdvances()
        {
            SaveCashAdvancesViewModel vm = new();
            vm.FromCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.ToAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("CashAdvances", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CashAdvances(SaveCashAdvancesViewModel vm)
        {
            vm.FromCreditCards = _creditCardService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            vm.ToAccounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();

            SaveCreditCardViewModel FromCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.FromCreditCardId);
            if (!ModelState.IsValid)
            {
                return View("CashAdvances", vm);
            }

            if (FromCreditCard.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta tarjeta de credito.";
                return View("CashAdvances", vm);
            }
            if (FromCreditCard.CreditLimit < vm.Amount)
            {
                ViewBag.Message = "Ese monto es mayor al limite de la tarjeta de credito, agrega uno menor al limite de RD$"+FromCreditCard.CreditLimit;
                return View("CashAdvances",vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("CashAdvances", vm);
            }

            if (FromCreditCard.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esta tarjeta de credito el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$"+FromCreditCard.Balance;
                return View("CashAdvances",vm);
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
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }


        public async Task<IActionResult> TransferBetweenAccounts()
        {
            SaveTransferBetweenAccountsViewModel vm =new();
            vm.Accounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            return View("TransferBetweenAccounts", vm);
        }

        [HttpPost]
        public async Task<IActionResult> TransferBetweenAccounts(SaveTransferBetweenAccountsViewModel vm)
        {
            vm.Accounts = _savingsAccountService.GetAllViewModel().Where(x => x.UserName == userViewModel.UserName).ToList();
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);

            if (!ModelState.IsValid)
            {
                return View("TransferBetweenAccounts", vm);
            }

            if (FromAccount.Balance == 0)
            {
                ViewBag.Message = "No tienes dinero en esta cuenta.";
                return View("TransferBetweenAccounts",vm);
            }
            if (vm.FromAccountId == vm.ToAccountId)
            {
                ViewBag.Message = "No se puede realizar una transaccion entre la misma cuenta.";
                return View("TransferBetweenAccounts", vm);
            }
            if (vm.Amount == 0)
            {
                ViewBag.Message = "El monto no puede ser cero, agrega uno mayor.";
                return View("TransferBetweenAccounts", vm);
            }

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción, agrega un monto igual o menor a RD$"+FromAccount.Balance +"";
                return View("TransferBetweenAccounts", vm);
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
            payment.UserName = userViewModel.UserName;

            //await _paymentService.Add(payment);

            return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
        }

    }


}

