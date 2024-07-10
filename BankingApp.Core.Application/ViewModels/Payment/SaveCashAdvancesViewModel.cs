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
    public class SaveCashAdvancesViewModel
    {
        [Range(400100100, 700100099, ErrorMessage = "Debe seleccionar una tarjeta de credito")]
        public int FromCreditCardId { get; set; }
        [Range(100100100, 400100099, ErrorMessage = "Debe seleccionar una cuenta")]
        public int ToAccountId { get; set; }
        [Required(ErrorMessage = "Debe ingresar un monto")]
        [DataType(DataType.Text)]
        public double Amount { get; set; }

        public List<CreditCardViewModel>? FromCreditCards { get; set; }

        public List<SavingsAccountViewModel>? ToAccounts { get; set; }
    }
}
