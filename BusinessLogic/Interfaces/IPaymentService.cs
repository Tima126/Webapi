using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPaymentService
    {

        Task<List<Payment>> GetAll();
        Task<Payment> GetById(int id);

        Task Create(Payment model);
        Task Update(Payment model);

        Task Delete(int id);
    }
}
