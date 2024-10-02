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
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace BusinessLogic.Services
{
    public class OrderService:IOrderService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<Order>> GetAll()
        {
            return await _repositoryWrapper.Order.FindAll();
        }


        public async Task<Order> GetById(int id)
        {
            var order = await _repositoryWrapper.Order
                .FindByCondition(x => x.OrderId == id);
            if (!order.Any())
            {
                throw new InvalidOperationException($"Order with id {id} not found.");
            }
            return order.First();
        }

        public async Task Create(Order model)
        {
            if(model== null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new OrderValidator();
            var validresult = validator.Validate(model);
            if (!validresult.IsValid)
            {
                var errorMessages = validresult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }

                await _repositoryWrapper.Order.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Order model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new OrderValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Order.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var order = await _repositoryWrapper.Order
                .FindByCondition(x => x.OrderId == id);
            if (!order.Any())
            {
                throw new InvalidOperationException($"Order with id {id} not found");
            }
            await _repositoryWrapper.Order.Delete(order.First());
            await _repositoryWrapper.Save();
        }
    }
}
