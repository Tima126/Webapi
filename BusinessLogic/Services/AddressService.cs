using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AddressService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AddressService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Address>> GetAll()
        {
            return _repositoryWrapper.Address.FindAll().ToListAsync();
        }


        public Task<Address> GetById(int id)
        {
            var address = _repositoryWrapper.Address.FinByCondition(x => x.AddressId == id).First();
            return Task.FromResult(address);
        }

        public Task Create(Address model)
        {
            _repositoryWrapper.Address.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Address model)
        {
            _repositoryWrapper.Address.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var address = _repositoryWrapper.Address.FinByCondition(x => x.AddressId == id).First();

            _repositoryWrapper.Address.Delete(address);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }


    }
}
