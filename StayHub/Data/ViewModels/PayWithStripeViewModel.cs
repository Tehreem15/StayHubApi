namespace StayHub.Data.ViewModels
{
    public class PayWithStripeViewModel
    {
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string Email { get; set; }
        public decimal price { get; set; }
        public string Currency { get; set; }
        public string ReferenceNumber { get; set; }

    }
}
