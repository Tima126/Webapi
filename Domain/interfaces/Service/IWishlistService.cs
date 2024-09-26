using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Service
{
    public interface IWishlistService
    {
        Task<List<Wishlist>> GetAll();
        Task<Wishlist> GetById(int id);

        Task Create(Wishlist model);
        Task Update(Wishlist model);

        Task Delete(int id);

    }
}
