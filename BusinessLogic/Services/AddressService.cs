using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class AddressService:IAddressService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AddressService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public async Task<List<Address>> GetAll()
        {
            return await _repositoryWrapper.Address.FindAll();
        }


        public async Task<Address> GetById(int id)
        {
            var address = await _repositoryWrapper.Address.FindByCondition(x => x.AddressId == id);
            return address.First();
        }

        public async Task Create(Address model)
        {
            await _repositoryWrapper.Address.Create(model);
            
           _repositoryWrapper.Save();
        }

        public async Task Update(Address model)
        {
            _repositoryWrapper.Address.Update(model);
            _repositoryWrapper.Save();
           
        }

        public async Task Delete(int id)
        {
            var address = await _repositoryWrapper.Address.FindByCondition(x => x.AddressId == id);

            _repositoryWrapper.Address.Delete(address.First());
            _repositoryWrapper.Save();
        }


    }
}
