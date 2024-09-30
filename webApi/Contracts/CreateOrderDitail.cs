namespace webApi.Contracts
{
    public class CreateOrderDitail
    {
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
