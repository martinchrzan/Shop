using ClientApp.Common;
using ClientApp.Helper;
using ClientApp.ViewModelContracts;
using ClientLib;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    [Export(typeof(IUserViewModel))]
    public class UserViewModel : ViewModelBase, IUserViewModel 
    {
        private readonly IPasswordHashProvider _passwordHashProvider;
        private readonly UserApi _userApi;

        private string _name;
        private string _password;
        private bool _processing;
        private bool _isError = false;
        
        [ImportingConstructor]
        public UserViewModel(IServerConnectionResolver serverConnectionResolver, IPasswordHashProvider passwordHashProvider)
        {
            _passwordHashProvider = passwordHashProvider;
            _userApi = new UserApi(serverConnectionResolver.GetServerApiBaseAddress());

            LogInUserCommand = new RelayCommand(ValidateNameAndPassword, LogInUser);
            RegisterUserCommand = new RelayCommand(ValidateNameAndPassword, RegisterUser);
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogInUserCommand { get; private set; }

        public ICommand RegisterUserCommand { get; private set; }

        public bool Processing
        {
            get
            {
                return _processing;
            }
            set
            {
                _processing = value;
                OnPropertyChanged();
            }
        }

        public bool IsError
        {
            get
            {
                return _isError;
            }
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<string> UserLogged;

        private bool ValidateNameAndPassword(object input)
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }

        private async void LogInUser(object input)
        {
            Processing = true;
            var result = await _userApi.LogInUser(Name, _passwordHashProvider.CalculateHash(Password));

            IsError = !result.IsValid;
            if(result.IsValid)
            {
                OnUserLogged(result.UserToken);
            }
            Processing = false;
        }

        private async void RegisterUser(object input)
        {
            Processing = true;
            IsError = !await _userApi.RegisterUser(Name, _passwordHashProvider.CalculateHash(Password));
            Processing = false;
        }
        
        private void OnUserLogged(string userToken)
        {
            UserLogged?.Invoke(this, userToken);
        }
    }
}
