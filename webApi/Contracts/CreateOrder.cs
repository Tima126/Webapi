namespace webApi.Contracts
{
    public class CreateOrder
    {
        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int? StatusId { get; set; }

        public int? DiscountId { get; set; }
    }
}
