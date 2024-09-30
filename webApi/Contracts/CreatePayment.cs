namespace webApi.Contracts
{
    public class CreatePayment
    {
        public int PaymentId { get; set; }

        public int? OrderId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}
