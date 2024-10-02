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
    public class ProductService:IProductService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<List<Product>> GetAll()
        {
            return await _repositoryWrapper.Product.FindAll();
        }


        public async Task<Product> GetById(int id)
        {
            var product = await _repositoryWrapper.Product
                .FindByCondition(x => x.ProductId == id);
            if (!product.Any())
            {
                throw new InvalidOperationException($"Product with id {id} not found.");
            }
            return product.First();
        }

        public async Task Create(Product model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validator = new ProductValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Product.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Product model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new ProductValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Product.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var product = await _repositoryWrapper.Product
                .FindByCondition(x => x.ProductId == id);
            if (!product.Any())
            {
                throw new InvalidOperationException($"Product with id {id} not found");
            }
            await _repositoryWrapper.Product.Delete(product.First());
            await _repositoryWrapper.Save();
        }

    }
}
