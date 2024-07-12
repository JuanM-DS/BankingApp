using BankingApp.Core.Application.Services;
using BankingApp.Core.Application.ViewModels.Product;
using BankingApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {

    }
}
