namespace webApi.Contracts
{
    public class CreateDiscount
    {
        public int DiscountId { get; set; }

        public string Code { get; set; } = null!;

        public decimal DiscountPercentage { get; set; }

        public DateOnly ExpiryDate { get; set; }
    }
}
