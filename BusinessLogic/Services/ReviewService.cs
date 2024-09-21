using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ReviewService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ReviewService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Review>> GetAll()
        {
            return _repositoryWrapper.Review.FindAll().ToListAsync();
        }


        public Task<Review> GetById(int id)
        {
            var review = _repositoryWrapper.Review.FinByCondition(x => x.ReviewId == id).First();
            return Task.FromResult(review);
        }

        public Task Create(Review model)
        {
            _repositoryWrapper.Review.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Review model)
        {
            _repositoryWrapper.Review.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var review = _repositoryWrapper.Review.FinByCondition(x => x.ReviewId == id).First();

            _repositoryWrapper.Review.Delete(review);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
