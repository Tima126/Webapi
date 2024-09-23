using Domain.interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SupplierRepository:RepositoryBase<Supplier>,ISupplierRepository
    {
        public SupplierRepository(FlowersStoreContext repositoriContext)
        : base(repositoriContext)
        {

        }
    }
}
