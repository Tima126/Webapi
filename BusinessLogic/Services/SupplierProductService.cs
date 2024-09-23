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
    public class SupplierProductService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SupplierProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<SupplierProduct>> GetAll()
        {
            return _repositoryWrapper.SupplierProduct.FindAll().ToListAsync();
        }


        public Task<SupplierProduct> GetById(int id)
        {
            var supplierproduct = _repositoryWrapper.SupplierProduct.FinByCondition(x => x.SupplierProductId == id).First();
            return Task.FromResult(supplierproduct);
        }

        public Task Create(SupplierProduct model)
        {
            _repositoryWrapper.SupplierProduct.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(SupplierProduct model)
        {
            _repositoryWrapper.SupplierProduct.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var supplierproduct = _repositoryWrapper.SupplierProduct.FinByCondition(x => x.SupplierProductId == id).First();

            _repositoryWrapper.SupplierProduct.Delete(supplierproduct);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
