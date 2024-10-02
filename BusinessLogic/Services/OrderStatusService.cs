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
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace BusinessLogic.Services
{
    public class OrderStatusService:IOrderStatusService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderStatusService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<OrderStatus>> GetAll()
        {
            return await _repositoryWrapper.OrderStatus.FindAll();
        }


        public async Task<OrderStatus> GetById(int id)
        {
            var orderStatus = await _repositoryWrapper.OrderStatus
                .FindByCondition(x => x.StatusId == id);
            if (!orderStatus.Any())
            {
                throw new InvalidOperationException($"Notificationtype with id {id} not found.");
            }
            return orderStatus.First();
        }

        public async Task Create(OrderStatus model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validator = new OrderStatusValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }
            await _repositoryWrapper.OrderStatus.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(OrderStatus model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new OrderStatusValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }
            await _repositoryWrapper.OrderStatus.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var orderStatus = await _repositoryWrapper.OrderStatus
                .FindByCondition(x => x.StatusId == id);
            if (!orderStatus.Any())
            {
                throw new InvalidOperationException($"NotificationType with id {id} not found");
            }
            await _repositoryWrapper.OrderStatus.Delete(orderStatus.First());
            await _repositoryWrapper.Save();
        }

    }
}
