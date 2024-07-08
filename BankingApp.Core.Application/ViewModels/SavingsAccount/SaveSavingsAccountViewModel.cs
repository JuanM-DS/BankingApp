using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.SavingsAccount
{
    public class SaveSavingsAccountViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string UserName { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
