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
    public class SupplierProductService:ISupplierProductService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SupplierProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<SupplierProduct>> GetAll()
        {
            return await _repositoryWrapper.SupplierProduct.FindAll();
        }


        public async Task<SupplierProduct> GetById(int id)
        {
            var supplierProduct = await _repositoryWrapper.SupplierProduct
                .FindByCondition(x => x.SupplierProductId == id);

            return supplierProduct.First();
        }

        public async Task Create(SupplierProduct model)
        {
            await _repositoryWrapper.SupplierProduct.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(SupplierProduct model)
        {
            await _repositoryWrapper.SupplierProduct.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var supplierProduct = await _repositoryWrapper.SupplierProduct
                .FindByCondition(x => x.SupplierProductId == id);

            await _repositoryWrapper.SupplierProduct.Delete(supplierProduct.First());
            await _repositoryWrapper.Save();
        }


    }
}
