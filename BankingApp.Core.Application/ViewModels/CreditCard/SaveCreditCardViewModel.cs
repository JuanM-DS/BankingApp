using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.CreditCard
{
    public class SaveCreditCardViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public double CreditLimit { get; set; }
    }
}
