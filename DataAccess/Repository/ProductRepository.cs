using Domain.interfaces.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(FlowersStoreContext repositoriContext)
        : base(repositoriContext)
        {

        }
    }
}
