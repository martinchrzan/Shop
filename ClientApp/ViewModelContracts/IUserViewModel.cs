using ClientApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.ViewModelContracts
{
    public interface IUserViewModel
    {
        string Name { get; set; }

        string Password { get; set; }

        ICommand LogInUserCommand { get; }
        
        ICommand RegisterUserCommand { get; }

        event EventHandler<string> UserLogged;

        bool Processing { get; }

        bool IsError { get; }
    }
}
