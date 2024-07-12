using BankingApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Domain.Common
{
    public class Product : BaseEntity
    {
        public byte Type { get; set; }
        //Navigation Property
        public ICollection<Payment> PaymentsTo { get; set; }
        public ICollection<Payment> PaymentsFrom { get; set; }

    }
}
