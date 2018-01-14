using ClientApp.Common;
using ClientApp.Helper;
using ClientApp.ViewModelContracts;
using ClientLib;
using System;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class ProductItemViewModel : ViewModelBase, IProductItemViewModel
    {
        private int _count;
        private ShopApi _shopApi;
        private string _userToken;

        public ProductItemViewModel(string userToken, int id, string name, string description, int count, decimal price, IServerConnectionResolver serverConnectionResolver)
        {
            _shopApi = new ShopApi(serverConnectionResolver.GetServerApiBaseAddress());
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            _count = count;
            _userToken = userToken;

            AddItemCommand = new RelayCommand((t) => true, AddItem);
            RemoveItemCommand = new RelayCommand(CanRemoveItem, RemoveItem);
        }
        
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged();
                // nasty way to force revaulation as it can fail sometimes with async changes
                (RemoveItemCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddItemCommand { get; private set; }

        public ICommand RemoveItemCommand { get; private set; }

        private async void AddItem(object input)
        {
            var success = await _shopApi.AddCustomerProduct(_userToken, Id, 1);
            if(success)
            {
                Count++;
            }
        }

        private async void RemoveItem(object input)
        {
            var success = await _shopApi.RemoveCustomerProduct(_userToken, Id, 1);
            if (success)
            {
                Count--;
            }
        }

        private bool CanRemoveItem(object input)
        {
            return Count > 0;
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}
