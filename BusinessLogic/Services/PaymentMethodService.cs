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
    public class PaymentMethodService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public PaymentMethodService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<PaymentMethod>> GetAll()
        {
            return _repositoryWrapper.PaymentMethod.FindAll().ToListAsync();
        }


        public Task<PaymentMethod> GetById(int id)
        {
            var paymentMethod = _repositoryWrapper.PaymentMethod.FinByCondition(x => x.PaymentMethodId == id).First();
            return Task.FromResult(paymentMethod);
        }

        public Task Create(PaymentMethod model)
        {
            _repositoryWrapper.PaymentMethod.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(PaymentMethod model)
        {
            _repositoryWrapper.PaymentMethod.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var paymentMethod = _repositoryWrapper.PaymentMethod.FinByCondition(x => x.PaymentMethodId == id).First();

            _repositoryWrapper.PaymentMethod.Delete(paymentMethod);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

    }
}
