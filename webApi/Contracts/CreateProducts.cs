namespace webApi.Contracts
{
    public class CreateProducts
    {
        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public int? CategoryId { get; set; }

        public int? DiscountId { get; set; }
    }
}
