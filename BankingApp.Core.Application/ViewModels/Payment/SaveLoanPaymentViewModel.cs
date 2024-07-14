using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Payment
{
    public class SaveLoanPaymentViewModel
    {
        [Range(700100100, 999999999, ErrorMessage = "Debe seleccionar un prestamo.")]
        public int ToLoanId { get; set; }
        [Range(100100098, 400100099, ErrorMessage = "Debe seleccionar una cuenta.")]
        public int FromAccountId { get; set; }
        [Required(ErrorMessage = "Debe ingresar un monto.")]
        [DataType(DataType.Text)]
        public double Amount { get; set; }

        public List<LoanViewModel>? ToLoans { get; set; }

        public List<SavingsAccountViewModel>? FromAccounts { get; set; }
    }
}
