using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class ReviewService:IReviewService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ReviewService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<List<Review>> GetAll()
        {
            return await _repositoryWrapper.Review.FindAll();
        }


        public async Task<Review> GetById(int id)
        {
            var reviewService = await _repositoryWrapper.Review
                .FindByCondition(x => x.ReviewId == id);

            return reviewService.First();
        }

        public async Task Create(Review model)
        {
            await _repositoryWrapper.Review.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(Review model)
        {
            _repositoryWrapper.Review.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var supplierProduct = await _repositoryWrapper.Review
                .FindByCondition(x => x.ReviewId == id);

            _repositoryWrapper.Review.Delete(supplierProduct.First());
            _repositoryWrapper.Save();
        }
    }
}
