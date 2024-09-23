using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderDetailsService
    {
        Task<List<OrderDetail>> GetAll();
        Task<OrderDetail> GetById(int id);

        Task Create(OrderDetail model);
        Task Update(OrderDetail model);

        Task Delete(int id);
    }
}
