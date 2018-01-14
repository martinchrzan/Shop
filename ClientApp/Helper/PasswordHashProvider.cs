using System;
using System.ComponentModel.Composition;

namespace ClientApp.Helper
{
    [Export(typeof(IPasswordHashProvider))]
    public class PasswordHashProvider : IPasswordHashProvider
    {
        public string CalculateHash(string password)
        {
            // caution, no hash calculation here!
            return password;
        }
    }
}
