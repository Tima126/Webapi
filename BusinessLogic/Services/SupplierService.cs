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
    public class SupplierService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SupplierService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Supplier>> GetAll()
        {
            return _repositoryWrapper.Supplier.FindAll().ToListAsync();
        }


        public Task<Supplier> GetById(int id)
        {
            var supplier = _repositoryWrapper.Supplier.FinByCondition(x => x.SupplierId == id).First();
            return Task.FromResult(supplier);
        }

        public Task Create(Supplier model)
        {
            _repositoryWrapper.Supplier.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Supplier model)
        {
            _repositoryWrapper.Supplier.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var supplier = _repositoryWrapper.Supplier.FinByCondition(x => x.SupplierId == id).First();

            _repositoryWrapper.Supplier.Delete(supplier);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

    }
}
