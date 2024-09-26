using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Service
{
    public interface IReviewService
    {

        Task<List<Review>> GetAll();
        Task<Review> GetById(int id);

        Task Create(Review model);
        Task Update(Review model);

        Task Delete(int id);
    }
}
