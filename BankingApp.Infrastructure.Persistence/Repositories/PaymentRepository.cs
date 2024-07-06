using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationContext _dbContext;

        public PaymentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Double> TransactionsTillCutoffDay(int CutoffDay, int CreditCardNumber)
        {
            int CurrentMonth = DateTime.Now.Month;
            int CurrentYear = DateTime.Now.Year;
            DateTime CurrentCutoffdate = new DateTime(CurrentYear, CurrentMonth, CutoffDay);
            DateTime PreviousCutoffdate = CurrentCutoffdate.AddMonths(-1);


            List<Payment> transactionsTillCutoffDay = await _dbContext.Set<Payment>().Where(ts => ts.FromProductId == CreditCardNumber)
                                                                                     .Where(ts => ts.CreatedTime > PreviousCutoffdate && ts.CreatedTime <= CurrentCutoffdate).ToListAsync();

            double TotalAmount =0;

            foreach (Payment payment in transactionsTillCutoffDay)
            {
                if (payment.Type == (byte)PaymentTypes.CashAdvance)
                {
                    TotalAmount -= payment.Amount;
                }
                else if (payment.Type == (byte)PaymentTypes.CreditCard)
                {
                    TotalAmount += payment.Amount;
                }
            }
            return TotalAmount;
        }
    }
}
