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
using FluentValidation;

namespace BusinessLogic.Services
{
    public class PaymentService: IPaymentService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public PaymentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Payment>> GetAll()
        {
            return await _repositoryWrapper.Payment.FindAll();
        }


        public async Task<Payment> GetById(int id)
        {
            var payment = await _repositoryWrapper.Payment
                .FindByCondition(x => x.PaymentId == id);
            
            if (!payment.Any())
            {
                throw new InvalidOperationException($"Payment with id {id} not found.");
            }

            return payment.First();
        }

        public async Task Create(Payment model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validator = new PaymentValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Payment.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Payment model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new PaymentValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Payment.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var payment = await _repositoryWrapper.Payment
                .FindByCondition(x => x.PaymentId== id);
            if (!payment.Any())
            {
                throw new InvalidOperationException($"Payment with id {id} not found");
            }
            await _repositoryWrapper.Payment.Delete(payment.First());
            await _repositoryWrapper.Save();
        }


    }
}
