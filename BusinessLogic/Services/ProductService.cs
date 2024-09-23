using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ProductService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Product>> GetAll()
        {
            return _repositoryWrapper.Product.FindAll().ToListAsync();
        }


        public Task<Product> GetById(int id)
        {
            var product = _repositoryWrapper.Product.FinByCondition(x => x.ProductId == id).First();
            return Task.FromResult(product);
        }

        public Task Create(Product model)
        {
            _repositoryWrapper.Product.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Product model)
        {
            _repositoryWrapper.Product.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var product = _repositoryWrapper.Product.FinByCondition(x => x.ProductId == id).First();

            _repositoryWrapper.Product.Delete(product);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

    }
}
