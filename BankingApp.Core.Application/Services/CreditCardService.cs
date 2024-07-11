using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Domain.Common;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreditCardService(ICreditCardRepository creditCardRepository, IMapper mapper, IProductRepository productRepository) : base(creditCardRepository, mapper)
        {
            _creditCardRepository = creditCardRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public override async Task Add(SaveCreditCardViewModel saveCreditCardViewModel)
        {
            var creditCard = _mapper.Map<CreditCard>(saveCreditCardViewModel);

            creditCard = await _creditCardRepository.AddAsync(creditCard);
            Product product = new();
            product.Id = creditCard.Id;
            product.UserName = creditCard.UserName;
            product.CreatedTime = creditCard.CreatedTime;
            product.CreatedBy = creditCard.CreatedBy;

            await _productRepository.AddAsync(product);
        }

        public List<CreditCardViewModel> GetAllByUserWithInclude(string userName)
        {
            var creditCards = _creditCardRepository.GetAllWithInclude();

            var creditCardsByUser = creditCards.Where(x => x.UserName == userName)
                                               .ToList();

            var creditcardsViewModel = _mapper.Map<List<CreditCardViewModel>>(creditCardsByUser);

            return creditcardsViewModel;
        }
    }
}
