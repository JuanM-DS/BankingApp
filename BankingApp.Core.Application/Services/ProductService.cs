using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.Product;
using BankingApp.Core.Domain.Common;

namespace BankingApp.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper) : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
    }
}
