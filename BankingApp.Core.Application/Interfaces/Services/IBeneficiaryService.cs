using BankingApp.Core.Domain.Entities;
using BankingApp.Core.Application.ViewModels.Beneficiary;

namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        Task<List<BeneficiaryViewModel>> BeneficiariesList();
        Task Delete(string userName, int accuntNumber);
    }
}
