using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IDicountServices
    {
        Task<List<Discount>> GetAll();
        Task<Discount> GetById(int id);

        Task Create(Discount model);
        Task Update(Discount model);

        Task Delete(int id);
    }
}
