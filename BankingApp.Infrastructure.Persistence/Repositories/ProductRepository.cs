using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.ViewModels.Product;
using BankingApp.Core.Domain.Common;
using BankingApp.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
