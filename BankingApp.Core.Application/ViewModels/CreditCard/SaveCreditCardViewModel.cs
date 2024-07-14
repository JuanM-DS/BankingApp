using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.ViewModels.CreditCard
{
    public class SaveCreditCardViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }

        [Required(ErrorMessage = "Debe colocar el límite de crédito de la tarjeta de crédito")]
        [Range(1000, double.MaxValue, ErrorMessage = "El límite de la tarjeta de crédito no puede ser menor a $RD1,000.00")]
        public double CreditLimit { get; set; }

        [Required(ErrorMessage = "Debe colocar el día de corte de la tarjeta de crédito")]
        [Range(1, 30, ErrorMessage = "El día de corte solo puede estar entre el 1 y el 30")]
        public byte CutoffDay { get; set; }
        public byte PaymentDay { get; set; }
        public string UserName { get; set; }
    }
}
