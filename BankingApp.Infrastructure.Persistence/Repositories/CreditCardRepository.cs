﻿using BankingApp.Core.Application.Enums;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Domain.Entities;
using BankingApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
    {
        private readonly ApplicationContext _dbContext;

        public CreditCardRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<CreditCard> AddAsync(CreditCard creditCard)
        {
            await base.AddAsync(creditCard);
            await _dbContext.SaveChangesAsync();
            return creditCard;
        }
        public override async Task UpdateAsync(CreditCard entity, int id)
        {
            var entry = await _dbContext.Set<CreditCard>().FindAsync(id);
            entry.Balance = entity.Balance;
            await base.UpdateAsync(entry, id);
        }
    }
}
