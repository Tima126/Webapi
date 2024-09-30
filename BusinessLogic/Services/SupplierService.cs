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
    public class SupplierService: ISupplierService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SupplierService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<Supplier>> GetAll()
        {
            return await _repositoryWrapper.Supplier.FindAll();
        }


        public async Task<Supplier> GetById(int id)
        {
            var supplier = await _repositoryWrapper.Supplier
                .FindByCondition(x => x.SupplierId == id);

            return supplier.First();
        }

        public async Task Create(Supplier model)
        {
            await _repositoryWrapper.Supplier.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Supplier model)
        {
            await _repositoryWrapper.Supplier.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var supplier = await _repositoryWrapper.Supplier
                .FindByCondition(x => x.SupplierId== id);

            await _repositoryWrapper.Supplier.Delete(supplier.First());
            await _repositoryWrapper.Save();
        }


    }
}
