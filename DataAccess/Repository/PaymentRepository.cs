using Domain.interfaces.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PaymentRepository:RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(FlowersStoreContext repositoriContext)
        : base(repositoriContext)
        {

        }
    }
}
