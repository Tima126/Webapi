using DataAccess.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.interfaces
{
    public interface IPaymentMethodRepository :IRepositoryBase<PaymentMethod>
    {
    }
}
