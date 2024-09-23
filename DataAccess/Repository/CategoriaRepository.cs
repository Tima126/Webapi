using Domain.interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoriaRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoriaRepository(FlowersStoreContext repositoriContext)
            : base(repositoriContext)
        {

        }
    }
}
