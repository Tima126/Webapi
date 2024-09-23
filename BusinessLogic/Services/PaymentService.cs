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
    public class PaymentService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public PaymentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Payment>> GetAll()
        {
            return _repositoryWrapper.Payment.FindAll().ToListAsync();
        }


        public Task<Payment> GetById(int id)
        {
            var payment = _repositoryWrapper.Payment.FinByCondition(x => x.PaymentId == id).First();
            return Task.FromResult(payment);
        }

        public Task Create(Payment model)
        {
            _repositoryWrapper.Payment.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Payment model)
        {
            _repositoryWrapper.Payment.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var payment = _repositoryWrapper.Payment.FinByCondition(x => x.PaymentId == id).First();

            _repositoryWrapper.Payment.Delete(payment);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

    }
}
