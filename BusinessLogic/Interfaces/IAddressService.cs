using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAddressService
    {

        Task<List<Address>> GetAll();
        Task<Address> GetById(int id);

        Task Create(Address model);
        Task Update(Address model);

        Task Delete(int id);



    }
}
