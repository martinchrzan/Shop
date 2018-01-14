using ClientApp.Common;
using ClientApp.ViewModelContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private CompositionContainer _container;
        private ViewModelBase _currentViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            Compose();
            CurrentViewModel = (ViewModelBase)UserViewModel;
            DataContext = this;
            UserViewModel.UserLogged += UserViewModel_UserLogged;
            BasketViewModel.LogOut += BasketViewModel_LogOut;
        }

        private void BasketViewModel_LogOut(object sender, EventArgs e)
        {
            CurrentViewModel = (ViewModelBase)UserViewModel;
        }

        private void UserViewModel_UserLogged(object sender, string e)
        {
            BasketViewModel.SetUserToken(e);
            CurrentViewModel = (ViewModelBase)BasketViewModel;
        }

        [Import]
        public IUserViewModel UserViewModel { get; set; }

        [Import]
        public IBasketViewModel BasketViewModel { get; set; }
        
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }
        
        private void Compose()
        {
            AssemblyCatalog catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            _container = new CompositionContainer(catalog);
            _container.SatisfyImportsOnce(this);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
