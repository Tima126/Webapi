using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Service
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetById(int id);

        Task Create(User model);
        Task Update(User model);

        Task Delete(int id);

    }
}
