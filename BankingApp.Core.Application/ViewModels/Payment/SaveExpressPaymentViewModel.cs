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
        [Range(100100100, 400100099, ErrorMessage = "Ese numero de cuenta no existe.")]
        public int ToAccountId {  get; set; }
        [Required(ErrorMessage = "Debe ingresar un monto.")]
        [DataType(DataType.Text)]
        public double Amount { get; set; }

        [Range(100100100, 400100099, ErrorMessage = "Debe seleccionar una cuenta.")]
        public int FromAccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<SavingsAccountViewModel>? FromAccounts { get; set; }

    }
}
