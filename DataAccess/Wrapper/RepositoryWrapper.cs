using Domain.interfaces;
using Domain.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Domain.interfaces.Repository;

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private FlowersStoreContext _repoContext;
        public RepositoryWrapper(FlowersStoreContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        private IUserRepository _user;
        private IAddressRepository _address;
        private ICategoryRepository _category;
        private IDiscountRepository _discount;
        private INotificationRepository _notification;
        private INotificationTypeRepository _notificationtype;
        private IOrderRepository _order;
        private IOrderDetailRepository _orderdetail;
        private IOrderStatusRepository _orderstatus;
        private IPaymentRepository _payment;
        private IPaymentMethodRepository _paymentmethods;
        private IProductRepository _product;
        private ISupplierRepository _supplier;
        private IReviewRepository _review;
        private ISupplierProductRepository _supplierproduct;
        private IWishlistRepository _wishlist;

        public IWishlistRepository Wishlist
        {
            get
            {
                if (_wishlist == null)
                {
                    _wishlist = new WishlistRepository(_repoContext);
                }
                return _wishlist;
            }
        }

        public ISupplierProductRepository SupplierProduct
        {
            get
            {
                if (_supplierproduct == null)
                {
                    _supplierproduct = new SupplierProductRepository(_repoContext);
                }
                return _supplierproduct;
            }
        }
        public IReviewRepository Review
        {
            get
            {
                if (_review == null)
                {
                    _review = new ReviewRepository(_repoContext);
                }
                return _review;
            }
        }


        public ISupplierRepository Supplier
        {
            get
            {
                if (_supplier == null)
                {
                    _supplier = new SupplierRepository(_repoContext);
                }
                return _supplier;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repoContext);
                }
                return _product;
            }
        }
        public IPaymentMethodRepository PaymentMethod
        {
            get
            {
                if (_paymentmethods == null)
                {
                    _paymentmethods = new PaymentMethodRepository(_repoContext);
                }
                return _paymentmethods;
            }
        }



        public IPaymentRepository Payment
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new PaymentRepository(_repoContext);
                }
                return _payment;
            }
        }

        public IOrderStatusRepository OrderStatus
        {
            get
            {
                if (_orderstatus == null)
                {
                    _orderstatus = new OrderStatusRepository(_repoContext);
                }
                return _orderstatus;
            }
        }



        public IOrderDetailRepository OrderDetail
        {
            get
            {
                if (_orderdetail == null)
                {
                    _orderdetail = new OrderDetailsRepository(_repoContext);
                }
                return _orderdetail;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepository(_repoContext);
                }
                return _order;
            }
        }

        public INotificationTypeRepository Notificationtype
        {
            get
            {
                if (_notificationtype == null)
                {
                    _notificationtype = new NotificationTypeRepository(_repoContext);
                }
                return _notificationtype;
            }
        }

        public INotificationRepository Notification
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepository(_repoContext);
                }
                return _notification;
            }
        }


        public IDiscountRepository Discount
        {
            get
            {
                if (_discount == null)
                {
                    _discount = new DiscountRepository(_repoContext);
                }
                return _discount;
            }
        }
        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoriaRepository(_repoContext);
                }
                return _category;
            }
        }

        public IAddressRepository Address
        {
            get
            {
                if (_address == null)
                {
                    _address = new AddressRepository(_repoContext);
                }
                return _address;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }






        public async Task Save()
        {
           await _repoContext.SaveChangesAsync();
        }

    }
}
