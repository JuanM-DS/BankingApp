﻿using BankingApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.Services;
using BankingApp.WebApp.Services;

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
            UserQueryFilter userFilter = new();
            userFilter.Role = RoleTypes.Client;

            var users = _userService.GetAll(userFilter).Result.Data;

            if (users != null)
            {
                ViewBag.ActiveClients = users.Where(x => x.Status == (byte)UserStatus.Active).Count();
                ViewBag.InactiveClients = users.Where(x => x.Status == (byte)UserStatus.Inactive).Count();
            }

            ViewBag.TotalProducts = _savingsAccountService.GetAllViewModel().Count() +
                                    _creditCardService.GetAllViewModel().Count() +
                                    _loanService.GetAllViewModel().Count();

            if (filterOption == null)
            {
                ViewBag.TransactionsTitle = "Transacciones de hoy";
                return View(await _paymentService.GetAllViewModel());
            }

            PaymentQueryFilters filters = new();
            switch (filterOption)
            {
                case 0:
                    ViewBag.TransactionsTitle = "Transferencias Históricas";
                    filters.PaymentTypes.Add(PaymentTypes.Transfer);
                    break;
                case 1:
                    ViewBag.TransactionsTitle = "Transferencias de hoy";
                    filters.PaymentTypes.Add(PaymentTypes.Transfer);
                    filters.Time = DateTime.Now;
                    break;
                case 2:
                    ViewBag.TransactionsTitle = "Pagos Históricos";
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToLoan);
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToCreditCard);
                    break;
                case 3:
                    ViewBag.TransactionsTitle = "Pagos de hoy";
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToLoan);
                    filters.PaymentTypes.Add(PaymentTypes.PaymentToCreditCard);
                    filters.Time = DateTime.Now;
                    break;
                default:
                    break;
            }

            List<PaymentViewModel> payments = await _paymentService.GetAllViewModel(filters);
            return View(payments);
        }

        public IActionResult Users()
        {
            return View(_userService.GetAll().Result.Data);
        }
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);


            var result = await _userService.RegisterAsync(viewModel);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View(viewModel);
            }
            var newUser = result.Data;

            //newUser.PhotoUrl = _imageService.UploadFile(viewModel.File, newUser.UserName, "Users");

            var updateResponse = await _userService.UpdateAsync(newUser);
            if (!updateResponse.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View();
            }

            return View(nameof(Index));
        }
    }
}
