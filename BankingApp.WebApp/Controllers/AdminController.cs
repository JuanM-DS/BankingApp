using BankingApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Core.Application.Helpers;
using BankingApp.Core.Application.ViewModels.User;
using BankingApp.Core.Application.QueryFilters;
using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.ViewModels.Payment;
using BankingApp.Core.Application.CustomEntities;
using BankingApp.Core.Application.Services;
using BankingApp.WebApp.Services;
using BankingApp.Core.Domain.Entities;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using BankingApp.Core.Application.ViewModels.Product;
using System.Drawing;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Domain.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        private readonly ImageServices _imageService;

        public AdminController(ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService, IPaymentService paymentService, IUserService userService, IHttpContextAccessor httpContextAccessor, ImageServices imageServices)
        {
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _imageService = imageServices;
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

        public async Task<IActionResult> Users()
        {
            return View((await _userService.GetAll()).Data);
        }

        [HttpPost]
        public async Task<IActionResult> FindByUserName(string filter)
        {
            var users = (await _userService.GetByMatchesAsync(filter)).Data;

            if (users != null)
            {
                return View("Users", users);
            }
            else
            {
                ViewBag.ErrorMessage = "No se encontraron usuarios con ese nombre";
            }
            return View("Users");
        }
        public IActionResult Register() { return View(); }

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

            newUser.PhotoUrl = _imageService.UploadFile(viewModel.File, newUser.UserName, "Users");
            newUser.Role = viewModel.Role;

            var updateResponse = await _userService.UpdateAsync(newUser);
            if (!updateResponse.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View();
            }

            if (viewModel.Role == RoleTypes.Client)
            {
                SaveSavingsAccountViewModel savingsAccount = new();

                savingsAccount.Balance = viewModel.InitialAmount ?? 0;
                savingsAccount.UserName = viewModel.UserName;
                savingsAccount.IsPrincipal = true;

                await _savingsAccountService.Add(savingsAccount);
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> EditUser(string id)
        {
            SaveUserViewModel user = (await _userService.GetSaveByIdAsync(id)).Data;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SaveUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var result = await _userService.UpdateAsync(viewModel);
            if (!result.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View(viewModel);
            }
            var newUser = result.Data;

            newUser.PhotoUrl = _imageService.UploadFile(viewModel.File, newUser.UserName, "Users", true, newUser.PhotoUrl);

            var updateResponse = await _userService.UpdateAsync(newUser);
            if (!updateResponse.Success)
            {
                ViewData["Success"] = result.Success;
                ViewData["Error"] = result.Error;
                return View();
            }

            if (viewModel.AditionalAmount != null && viewModel.AditionalAmount > 0)
            {
                SaveSavingsAccountViewModel principalAccount = await _savingsAccountService.GetPrincipalAccount(newUser.UserName);

                principalAccount.Balance = (double)viewModel.AditionalAmount;

                await _savingsAccountService.Update(principalAccount, principalAccount.Id);
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> ChangeUserStatus(string id)
        {
            SaveUserViewModel user = (await _userService.GetSaveByIdAsync(id)).Data;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(SaveUserViewModel userVM)
        {
            userVM = (await _userService.GetSaveByIdAsync(userVM.Id)).Data;

            if (userVM.Status == (byte)UserStatus.Active)
            {
                userVM.Status = (byte)UserStatus.Inactive;
            }
            else
            {
                userVM.Status = (byte)UserStatus.Active;
            }

            await _userService.UpdateAsync(userVM);

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Products(string id, string message = "")
        {
            ProductsViewModel products = new();
            products.User = (await _userService.GetByIdAsync(id)).Data;
            products.Loans = _loanService.GetAllViewModel().Where(l => l.UserName == products.User.UserName).ToList();
            products.SavingsAccounts = _savingsAccountService.GetAllViewModel().Where(s => s.UserName == products.User.UserName).ToList();
            products.CreditCards = _creditCardService.GetAllViewModel().Where(c => c.UserName == products.User.UserName).ToList();

            ViewBag.Message = message;
            return View(products);
        }

        public async Task<IActionResult> AddAccount(string userId)
        {
            SaveSavingsAccountViewModel account = new();
            ViewBag.User = (await _userService.GetByIdAsync(userId)).Data;
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(SaveSavingsAccountViewModel savingsAccount)
        {
            savingsAccount.IsPrincipal = false;
            await _savingsAccountService.Add(savingsAccount);
            
            var user = _userService.GetByNameAsync(savingsAccount.UserName).Data;
            return RedirectToAction("Products", new {id = user.Id, message = "Cuenta asignada exitosamente" });
        }

        public async Task<IActionResult> AddCreditCard(string userId)
        {
            SaveCreditCardViewModel creditCard = new();
            ViewBag.User = (await _userService.GetByIdAsync(userId)).Data;
            return View(creditCard);
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditCard(SaveCreditCardViewModel creditCard)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.User = (_userService.GetByNameAsync(creditCard.UserName)).Data;
                return View(creditCard);
            }

            if ((int)creditCard.CutoffDay > 5) creditCard.PaymentDay = (byte)((int)creditCard.CutoffDay - 5);
            
            else creditCard.PaymentDay = (byte)(30 - (5 - (int)creditCard.CutoffDay));

            await _creditCardService.Add(creditCard);

            var user = _userService.GetByNameAsync(creditCard.UserName).Data;
            return RedirectToAction("Products", new { id = user.Id, message = "Tarjeta de crédito asignada exitosamente" });
        }

        public async Task<IActionResult> AddLoan(string userId)
        {
            SaveLoanViewModel loan = new();
            ViewBag.User = (await _userService.GetByIdAsync(userId)).Data;
            return View(loan);
        }

        [HttpPost]
        public async Task<IActionResult> AddLoan(SaveLoanViewModel loan)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.User = (_userService.GetByNameAsync(loan.UserName)).Data;
                return View(loan);
            }

            loan.PaymentDay = (byte)DateTime.Now.Day;
            await _loanService.Add(loan);

            await _paymentService.Disbursement(loan);

            var user = _userService.GetByNameAsync(loan.UserName).Data;
            
            return RedirectToAction("Products", new { id = user.Id, message = "Préstamo asignado exitosamente" });
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            var savingsAccount = await _savingsAccountService.GetByIdSaveViewModel(id);
            var user = _userService.GetByNameAsync(savingsAccount.UserName).Data;

            if (savingsAccount.IsPrincipal) return RedirectToAction("Products", new { id = user.Id, message = $"La cuenta {savingsAccount.Id} no puede ser eliminada porque es la principal del cliente" });

            if (savingsAccount.Balance > 0)
            {
                await _paymentService.SavingsAccountDeletion(savingsAccount);
            }

            await _savingsAccountService.Delete(savingsAccount.Id);

            return RedirectToAction("Products", new { id = user.Id, message = "Cuenta eliminada exitosamente" });

        }
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _loanService.GetByIdSaveViewModel(id);
            var user = _userService.GetByNameAsync(loan.UserName).Data;

            if (loan.Balance > 0) return RedirectToAction("Products", new { id = user.Id, message = $"El préstamo {loan.Id} no puede ser eliminado porque tiene un balance pendiente" });

            await _loanService.Delete(loan.Id);
            return RedirectToAction("Products", new { id = user.Id, message = $"Préstamo eliminado satisfactoriamente" });
        }

        public async Task<IActionResult> DeleteCreditCard(int id)
        {
            var creditCard = await _creditCardService.GetByIdSaveViewModel(id);
            var user = _userService.GetByNameAsync(creditCard.UserName).Data;

            if (creditCard.Balance > 0) return RedirectToAction("Products", new { id = user.Id, message = $"La tarjeta de crédito {creditCard.Id} no puede ser eliminada porque tiene un balance pendiente" });

            await _creditCardService.Delete(creditCard.Id);
            return RedirectToAction("Products", new { id = user.Id, message = $"Tarjeta de crédito eliminada satisfactoriamente" });
        }
    }
}
