namespace Domain.interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IAddressRepository Address { get; }
        ICategoryRepository Category { get; }
        IDiscountRepository Discount { get; }
        INotificationRepository Notification { get; }
        INotificationTypeRepository Notificationtype { get; }
        IOrderRepository Order { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderStatusRepository OrderStatus { get; }
        IPaymentRepository Payment { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        IProductRepository Product { get; }
        ISupplierRepository Supplier { get; }
        IReviewRepository Review { get; }
        ISupplierProductRepository SupplierProduct { get; }
        IWishlistRepository Wishlist { get; }
        Task Save();
    }
}
