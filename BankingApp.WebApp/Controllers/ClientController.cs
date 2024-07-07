using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class ClientController : Controller
    {
        public readonly IBeneficiaryService _beneficiaryService;
        public readonly ISavingsAccountService _savingsAccountService;
        public readonly ICreditCardService _creditCardService;
        public readonly ILoanService _loanService;
        public readonly IPaymentService _paymentService;

        public ClientController(IBeneficiaryService beneficiaryService,ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService,IPaymentService paymentService)
        {
            _beneficiaryService = beneficiaryService;
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _paymentService = paymentService;
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

            if(savingsAccount == null)
            {
                ViewBag.Message = "El numero de cuenta no existe";
                return View("Beneficiary");
            }
            SaveBeneficiaryViewModel vm = new();
            vm.AccountNumber = accountNumber;
            vm.BeneficiaryUserName = savingsAccount.UserName;
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
        public async Task<IActionResult> DeletePost(SaveBeneficiaryViewModel vm)
        {
            await _beneficiaryService.Delete(vm.AccountNumber);
            return RedirectToRoute(new { controller = "Client", action = "Beneficiary" });
        }

        public async Task<IActionResult> ExpressPayment()
        {
            SaveExpressPaymentViewModel vm = new();
            vm.FromAccounts = await _savingsAccountService.GetAllViewModel();
            return View("ExpressPayment",vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(SaveExpressPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("ExpressPayment", vm);
            }
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccount);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);
            if (ToAccount == null)
            {
                ViewBag.Message = "El número de cuenta no existe, se ha cancelado la transacción.";
                return View();
            }

            if(FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción";
                return View();
            }
            return View("ConfirmTransactionExpress", vm);
        }

        public async Task<IActionResult> ConfirmTransactionExpress(SaveExpressPaymentViewModel vm)
        {

            return View("ConfirmTransactionExpress", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTransactionExpressPost(SaveExpressPaymentViewModel vm)
        {
            SaveSavingsAccountViewModel ToAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.ToAccount);
            ToAccount.Balance += vm.Amount;
            await _savingsAccountService.Update(ToAccount,ToAccount.Id);
            SaveSavingsAccountViewModel FromAccount = await _savingsAccountService.GetByIdSaveViewModel(vm.FromAccountId);
            FromAccount.Balance -= vm.Amount;
            await _savingsAccountService.Update(FromAccount,FromAccount.Id);


            return View("Index", vm);
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

            if (FromAccount.Balance < vm.Amount)
            {
                ViewBag.Message = "No tienes en esa cuenta el dinero suficiente para realizar la transacción";
                return View();
            }
            return View("CreditCardPayment", vm);

            SaveCreditCardViewModel ToCreditCard = await _creditCardService.GetByIdSaveViewModel(vm.ToCreditCardId);
            ToCreditCard.Balance += vm.Amount;
            FromAccount.Balance -= vm.Amount;
            if (ToCreditCard.Balance > ToCreditCard.CreditLimit)
            {
               vm.Amount = ToCreditCard.Balance - ToCreditCard.CreditLimit;
            }
            FromAccount.Balance += vm.Amount;
            await _creditCardService.Update(ToCreditCard, ToCreditCard.Id);
            await _savingsAccountService.Update(FromAccount, FromAccount.Id);


            return View("Index", vm);
        }


        public async Task<IActionResult> LoanRepayment()
        {
            return View();
        }


       

    }
}
