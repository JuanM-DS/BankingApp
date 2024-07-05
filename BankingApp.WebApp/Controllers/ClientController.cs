using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Beneficiary;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.WebApp.Controllers
{
    public class ClientController : Controller
    {
        public readonly IBeneficiaryService _beneficiaryService;

        public ClientController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        public async Task <IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> Beneficiary()
        {
            List<BeneficiaryViewModel> Vm = await _beneficiaryService.GetAllViewModel();
            return View("Beneficiary",Vm);
        }
    }
}
