using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Service
{
    public interface IOrderStatusService
    {
        Task<List<OrderStatus>> GetAll();
        Task<OrderStatus> GetById(int id);

        Task Create(OrderStatus model);
        Task Update(OrderStatus model);

        Task Delete(int id);
    }
}
