using Domain.interfaces.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SupplierProductRepository : RepositoryBase<SupplierProduct>, ISupplierProductRepository
    {
        public SupplierProductRepository(FlowersStoreContext repositoriContext)
: base(repositoriContext)
        {

        }
    }
}
