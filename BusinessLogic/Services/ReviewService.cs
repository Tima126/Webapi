using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;
using BusinessLogic.Validation;
using FluentValidation;

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

            if (!reviewService.Any())
            {
                throw new InvalidOperationException($"Review with id {id} not found.");
            }
            return reviewService.First();
        }

        public async Task Create(Review model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validator = new ReviewValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Review.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Review model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new ReviewValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Review.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var supplierProduct = await _repositoryWrapper.Review
                .FindByCondition(x => x.ReviewId == id);
            if (!supplierProduct.Any())
            {
                throw new InvalidOperationException($"Review with id {id} not found");
            }
            await _repositoryWrapper.Review.Delete(supplierProduct.First());
            await _repositoryWrapper.Save();
        }
    }
}
