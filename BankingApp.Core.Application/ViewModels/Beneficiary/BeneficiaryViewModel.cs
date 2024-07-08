using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AccountNumber { get; set; }
        public string UserName { get; set; }
        public string BeneficiaryUserName { get; set; }
    }
}
