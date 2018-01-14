using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.ViewModelContracts
{
    public interface IBasketViewModel
    {
        void SetUserToken(string userToken);
        
        ICommand ClearAllCommand { get; }

        ICommand PayCommand { get; }

        ICommand LogOutCommand { get; }

        event EventHandler LogOut;

        ObservableCollection<IProductItemViewModel> ProductItemViewModels { get; }
    }
}
