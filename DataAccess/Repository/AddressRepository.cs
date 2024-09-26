using DataAccess.Repository;
using Domain.interfaces.Repository;
using Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(FlowersStoreContext repositoriContext)
        : base(repositoriContext)
        {

        }
    }
}
