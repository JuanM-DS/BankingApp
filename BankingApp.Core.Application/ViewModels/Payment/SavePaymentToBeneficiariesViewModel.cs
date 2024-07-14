using BankingApp.Core.Application.ViewModels.Beneficiary;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Payment
{
    public class SavePaymentToBeneficiariesViewModel
    {
        [Range(100100098, 400100099, ErrorMessage = "Debe seleccionar un beneficiario.")]
        public int ToBeneficiaryId { get; set; }
        [Range(100100098, 400100099, ErrorMessage = "Debe seleccionar una cuenta.")]
        public int FromAccountId { get; set; }
        [Required(ErrorMessage = "Debe ingresar un monto.")]
        [DataType(DataType.Text)]
        public double Amount { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<BeneficiaryViewModel>? ToBeneficiaries { get; set; }

        public List<SavingsAccountViewModel>? FromAccounts { get; set; }
    }
}
