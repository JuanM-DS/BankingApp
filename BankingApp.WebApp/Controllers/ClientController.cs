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

        public async Task<IActionResult> Beneficiary(string message = "")
        {
            ViewBag.Message = message;
            List<BeneficiaryViewModel> Vm = (await _beneficiaryService.BeneficiariesList()).Where(b => b.UserName == userViewModel.UserName).ToList();
            return View("Beneficiary",Vm);
        }

        public async Task<IActionResult> CreateBeneficiary(int accountNumber)
        {
            Response<List<BeneficiaryViewModel>> response = await _beneficiaryService.CreateBeneficiary(accountNumber,userViewModel.UserName);
            ViewBag.Message = response.Error;
            if(response.Success == true)
            {
                return RedirectToAction(response.View);
            }
            else
            {
                return View(response.View, response.Data);
            }
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

            Response<SaveExpressPaymentViewModel> response = await _paymentService.ExpressPayment(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction(response.View, response.Data);
            }
            else
            {
                return View(response.View, response.Data);
            }
        }

        public async Task<IActionResult> ConfirmTransactionExpress(SaveExpressPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccountId);
            List<UserViewModel> users = (await _userService.GetByNameAsync(savingsAccount.UserName)).Data.ToList();
            vm.FirstName = users[0].FirstName;
            vm.LastName = users[0].LastName;

            return View("ConfirmTransactionExpress", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTransactionExpressPost(SaveExpressPaymentViewModel vm)
        {
             await _paymentService.ConfirmTransactionExpressPost(vm, userViewModel.UserName);

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

            if (!ModelState.IsValid)
            {
                return View("CreditCardPayment", vm);
            }
            Response<SaveCreditCardPaymentViewModel> response = await _paymentService.CreditCardPayment(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
            }
            else
            {
                return View(response.View, response.Data);
            }
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

            if (!ModelState.IsValid)
            {
                return View("LoanPayment", vm);
            }
            Response<SaveLoanPaymentViewModel> response = await _paymentService.LoanPayment(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
            }
            else
            {
                return View(response.View, response.Data);
            }

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

            if (!ModelState.IsValid)
            {
                return View("PaymentToBeneficiaries", vm);
            }
            Response<SavePaymentToBeneficiariesViewModel> response = await _paymentService.PaymentToBeneficiaries(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction(response.View, response.Data);
            }
            else
            {
                return View(response.View, response.Data);
            }
        }

        public async Task<IActionResult> ConfirmTransactionBeneficiary(SavePaymentToBeneficiariesViewModel vm)
        {
            SaveSavingsAccountViewModel savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToBeneficiaryId);
            var user = _userService.GetByNameAsync(savingsAccount.UserName);
            vm.FirstName = user.Data.FirstName;
            vm.LastName = user.Data.LastName;

            return View("ConfirmTransactionBeneficiary", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTransactionBeneficiaryPost(SavePaymentToBeneficiariesViewModel vm)
        {
            await _paymentService.ConfirmTransactionBeneficiaryPost(vm, userViewModel.UserName);

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

            if (!ModelState.IsValid)
            {
                return View("CashAdvances", vm);
            }
            Response<SaveCashAdvancesViewModel> response = await _paymentService.CashAdvances(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
            }
            else
            {
                return View(response.View, response.Data);
            }

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
            Response<SaveTransferBetweenAccountsViewModel> response = await _paymentService.TransferBetweenAccounts(vm, userViewModel.UserName);
            ViewBag.Message = response.Error;
            if (response.Success == true)
            {
                return RedirectToAction("Index", new { message = "Se ha realizado la transacción" });
            }
            else
            {
                return View(response.View, response.Data);
            }
        }

    }


}

