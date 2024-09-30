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
    public class PaymentMethodService: IPaymentMethodService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public PaymentMethodService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<PaymentMethod>> GetAll()
        {
            return await _repositoryWrapper.PaymentMethod.FindAll();
        }


        public async Task<PaymentMethod> GetById(int id)
        {
            var paymentMethod = await _repositoryWrapper.PaymentMethod
                .FindByCondition(x => x.PaymentMethodId == id);

            return paymentMethod.First();
        }

        public async Task Create(PaymentMethod model)
        {
            await _repositoryWrapper.PaymentMethod.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(PaymentMethod model)
        {
            await _repositoryWrapper.PaymentMethod.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var PaymentMethod = await _repositoryWrapper.PaymentMethod
                .FindByCondition(x => x.PaymentMethodId == id);

            await _repositoryWrapper.PaymentMethod.Delete(PaymentMethod.First());
            await _repositoryWrapper.Save();
        }

    }
}
