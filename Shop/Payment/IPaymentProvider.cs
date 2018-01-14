namespace Shop.Payment
{
    public interface IPaymentProvider
    {
        bool ProcessPayment(string userToken);
    }
}
