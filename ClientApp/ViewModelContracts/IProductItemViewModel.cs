using System.Windows.Input;

namespace ClientApp.ViewModelContracts
{
    public interface IProductItemViewModel
    {
        int Id { get; }

        string Name { get; }

        string Description { get; }

        int Count { get; set; }

        decimal Price { get; }

        ICommand AddItemCommand { get; }

        ICommand RemoveItemCommand { get; }

        void Clear();
    }
}
