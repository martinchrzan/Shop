using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.ViewModelContracts;
using System.ComponentModel.Composition;
using ClientApp.ViewModelsContracts;
using ClientApp.Helper;

namespace ClientApp.ViewModels
{
    [Export(typeof(IProductItemViewModelFactory))]
    public class ProductItemViewModelFactory : IProductItemViewModelFactory
    {
        private readonly IServerConnectionResolver _serverConnectionResolver;

        [ImportingConstructor]
        public ProductItemViewModelFactory(IServerConnectionResolver serverConnectionResolver)
        {
            _serverConnectionResolver = serverConnectionResolver;
        }

        public IProductItemViewModel GetProductViewModel(string userToken, int productId, string name, string description, int count, decimal price)
        {
            return new ProductItemViewModel(userToken, productId, name, description, count, price, _serverConnectionResolver);
        }
    }
}
