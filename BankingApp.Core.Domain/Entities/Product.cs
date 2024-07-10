using BankingApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Domain.Entities
{
    public class Product: BaseEntity
    {
        public double Balance { get; set; }

        //Navigation Property
        public ICollection<Payment> PaymentsTo { get; set; }
        public ICollection<Payment> PaymentsFrom { get; set; }

    }
}
