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
using BusinessLogic.Validation;
using FluentValidation;

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
            if (!address.Any())
            {
                throw new InvalidOperationException($"Address with id {id} not found");
            }
            
            return address.First();
        }

        public async Task Create(Address model)
        {

            var validator = new AddressValidator();
            var validattorResult = validator.Validate(model);

            if (!validattorResult.IsValid)
            {
                var errorMassages = validattorResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMassagesString = string.Join(", ", errorMassages);
                throw new ValidationException(errorMassagesString);
            }


            await _repositoryWrapper.Address.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Address model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new AddressValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }

            await _repositoryWrapper.Address.Update(model);
            await _repositoryWrapper.Save();
           
        }

        public async Task Delete(int id)
        {
            var address = await _repositoryWrapper.Address.FindByCondition(x => x.AddressId == id);
            if (!address.Any())
            {
                throw new InvalidOperationException($"Address with id {id} not found");
            }
            await _repositoryWrapper.Address.Delete(address.First());
            await _repositoryWrapper.Save();
        }


    }
}
