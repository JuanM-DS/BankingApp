using BankingApp.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Payment
{
    public class SaveExpressPaymentViewModel
    {
        [Required(ErrorMessage = "Debe ingresar una cuenta")]
        [DataType(DataType.Text)]
        public int ToAccount {  get; set; }
        [Required(ErrorMessage = "Debe ingresar un monto")]
        [DataType(DataType.Text)]
        public double Amount { get; set; }
        public List<SavingsAccountViewModel> FromAccounts { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una cuenta")]
        [DataType(DataType.Text)]
        public int FromAccountId { get; set; }
    }
}
