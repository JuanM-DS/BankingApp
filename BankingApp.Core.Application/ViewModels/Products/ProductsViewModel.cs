using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Application.ViewModels.Loan;
using BankingApp.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Product
{
    public class ProductsViewModel
    {
       public List<SavingsAccountViewModel> savingsAccounts { get; set; }
       public List<CreditCardViewModel> CreditCards { get; set; }
       public List<LoanViewModel> Loans { get; set; }
    }
}
