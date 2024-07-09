using BankingApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.ViewModels.Payment;

namespace BankingApp.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public AdminController(ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService, IPaymentService paymentService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index(int? filterOption)
        {
            if (filterOption == null)
            {
                ViewBag.ShowTransactions = false;
                return View(await _paymentService.GetAllViewModel());
            }

            PaymentQueryFilters filters = new();
            switch (filterOption)
            {
                case 0:
                    filters.PaymentTypes.Add(PaymentTypes.Transfer);
                    break;
                case 1:
                    filters.PaymentTypes.Add(PaymentTypes.Transfer);
                    filters.Time = DateTime.Now;
                    break;
                case 2:
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToLoan);
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToCreditCard);
                    break;
                case 3:
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToLoan);
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToCreditCard);
                    filters.Time = DateTime.Now;
                    break;
                default:
                    break;
            }

            List<PaymentViewModel> payments = await _paymentService.GetAllViewModel(filters);
            ViewBag.ShowTransactions = true;
            return View(payments);
        }
    }
}
