﻿namespace BankingApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        Task Add(SaveViewModel vm);
        Task Update(SaveViewModel vm, int id);
        Task Delete(int id);
        List<ViewModel> GetAllViewModel();
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
    }
}
