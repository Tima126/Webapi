using Domain.interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailsRepository(FlowersStoreContext repositoriContext)
       : base(repositoriContext)
        {

        }
    }
}
