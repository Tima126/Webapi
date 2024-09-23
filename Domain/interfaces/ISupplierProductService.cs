using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISupplierProductService
    {

        Task<List<SupplierProduct>> GetAll();
        Task<SupplierProduct> GetById(int id);

        Task Create(SupplierProduct model);
        Task Update(SupplierProduct model);

        Task Delete(int id);
    }
}
