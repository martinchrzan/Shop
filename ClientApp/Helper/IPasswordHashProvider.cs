namespace ClientApp.Helper
{
    public interface IPasswordHashProvider
    {
        string CalculateHash(string password);
    }
}
