using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.Loan
{
    public class SaveLoanViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el monto del préstamo")]
        [Range(1000, double.MaxValue, ErrorMessage = "El monto del préstamo no puede ser menor a $RD1,000.00")]
        public double Principal { get; set; }
        public double Balance { get; set; }

        [Required(ErrorMessage = "Debe colocar la tasa de interés del préstamo")]
        [Range(0, 25, ErrorMessage = "La tasa de interés no puede ser mayor a 25%. No somos el Popular, bro. :v")]
        public double InterestRate { get; set; }

        [Required(ErrorMessage = "Debe colocar el plazo del préstamo")]
        [Range(6, 300, ErrorMessage = "El plazo del préstamo no puede ser menor a 6 meses ni mayor a 25 años")]
        public int Term { get; set; }
        public double Installment { get; set; }
        public byte PaymentDay { get; set; }
        public string UserName { get; set; }
    }
}
