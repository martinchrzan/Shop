namespace Shop.Payment
{
    /// <summary>
    /// Dummy payment implementation
    /// </summary>
    public class PaymentProvider : IPaymentProvider
    {
        public bool ProcessPayment(string userToken)
        {
            return true;
        }
    }
}
