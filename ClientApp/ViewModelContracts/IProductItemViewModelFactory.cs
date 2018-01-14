using ClientApp.ViewModelContracts;

namespace ClientApp.ViewModelsContracts
{
    public interface IProductItemViewModelFactory
    {
        IProductItemViewModel GetProductViewModel(string userToken, int productId, string name, string description, int count, decimal price);
    }
}
