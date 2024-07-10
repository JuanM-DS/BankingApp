using AutoMapper;
using BankingApp.Core.Application.Interfaces.Repositories;
using BankingApp.Core.Application.Interfaces.Services;
using BankingApp.Core.Application.ViewModels.CreditCard;
using BankingApp.Core.Domain.Entities;

namespace BankingApp.Core.Application.Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IMapper _mapper;

        public CreditCardService(ICreditCardRepository creditCardRepository, IMapper mapper) : base(creditCardRepository, mapper)
        {
            _creditCardRepository = creditCardRepository;
            _mapper = mapper;
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
