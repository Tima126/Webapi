using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

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

            return product.First();
        }

        public async Task Create(Product model)
        {
            await _repositoryWrapper.Product.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(Product model)
        {
            _repositoryWrapper.Product.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var product = await _repositoryWrapper.Product
                .FindByCondition(x => x.ProductId == id);

            _repositoryWrapper.Product.Delete(product.First());
            _repositoryWrapper.Save();
        }

    }
}
