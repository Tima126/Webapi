namespace webApi.Contracts
{
    public class CreateWishlist
    {
        public int WishlistId { get; set; }

        public int? UserId { get; set; }

        public int? ProductId { get; set; }
    }
}
