using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

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
            _repositoryWrapper.Save();
        }



        public async Task Update(PaymentMethod model)
        {
            _repositoryWrapper.PaymentMethod.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var PaymentMethod = await _repositoryWrapper.PaymentMethod
                .FindByCondition(x => x.PaymentMethodId == id);

            _repositoryWrapper.PaymentMethod.Delete(PaymentMethod.First());
            _repositoryWrapper.Save();
        }

    }
}
