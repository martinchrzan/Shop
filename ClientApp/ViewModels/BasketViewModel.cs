using ClientApp.Common;
using ClientApp.Helper;
using ClientApp.ViewModelContracts;
using ClientApp.ViewModelsContracts;
using ClientLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    [Export(typeof(IBasketViewModel))]
    public class BasketViewModel : ViewModelBase, IBasketViewModel
    {
        private readonly ShopApi _shopApi;
        private string _userToken;
        private readonly IProductItemViewModelFactory _productItemViewModelFactory;
        
        [ImportingConstructor]
        public BasketViewModel(IServerConnectionResolver serverConnectionResolver, IProductItemViewModelFactory productItemViewModelFactory)
        {
            _shopApi = new ShopApi(serverConnectionResolver.GetServerApiBaseAddress());
            _productItemViewModelFactory = productItemViewModelFactory;
            ClearAllCommand = new RelayCommand((t) => true, ClearAll);
            LogOutCommand = new RelayCommand((t) => true, OnLogOut);
            ProductItemViewModels = new ObservableCollection<IProductItemViewModel>();
        }

        public event EventHandler LogOut;

        public ICommand ClearAllCommand { get; private set; }

        public ICommand PayCommand { get; private set; }

        public ObservableCollection<IProductItemViewModel> ProductItemViewModels { get; private set; }

        public ICommand LogOutCommand { get; private set; }

        public async void SetUserToken(string userToken)
        {
            _userToken = userToken;
            ProductItemViewModels.Clear();
            await InitializeProducts();
        }

        private async Task InitializeProducts()
        {
            var products = await _shopApi.ListProducts();
            var userProducts = await _shopApi.GetCustomerProducts(_userToken);

            foreach(var product in products)
            {
                var count = 0;
                var productCount = userProducts.Products.FirstOrDefault(it => it.ProductId == product.Id);
                if(productCount != null)
                {
                    count = productCount.Count;
                }

                var productItem = _productItemViewModelFactory.GetProductViewModel(_userToken, product.Id, product.Name, product.Description, count, product.Price);

                ProductItemViewModels.Add(productItem);
            }
        }
        
        private async void ClearAll(object value)
        {
            var result = await _shopApi.ClearProducts(_userToken);
            if(result)
            {
                foreach (var product in ProductItemViewModels)
                {
                    product.Clear();
                }
            }
        }

        private void OnLogOut(object input)
        {
            LogOut?.Invoke(this, EventArgs.Empty);
        }
    }
}
