using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Loan
{
    public class SaveLoanViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public int Term { get; set; }
        public double Installment { get; set; }
        public byte PaymentDay { get; set; }
        public string UserName { get; set; }
    }
}
