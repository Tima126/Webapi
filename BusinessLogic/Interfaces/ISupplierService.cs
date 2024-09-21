using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAll();
        Task<Supplier> GetById(int id);

        Task Create(Supplier model);
        Task Update(Supplier model);

        Task Delete(int id);
    }
}
