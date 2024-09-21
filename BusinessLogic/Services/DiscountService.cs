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
    public class DiscountService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public DiscountService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Discount>> GetAll()
        {
            return _repositoryWrapper.Discount.FindAll().ToListAsync();
        }


        public Task<Discount> GetById(int id)
        {
            var discount = _repositoryWrapper.Discount.FinByCondition(x => x.DiscountId == id).First();
            return Task.FromResult(discount);
        }

        public Task Create(Discount model)
        {
            _repositoryWrapper.Discount.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Discount model)
        {
            _repositoryWrapper.Discount.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var discount = _repositoryWrapper.Discount.FinByCondition(x => x.DiscountId == id).First();

            _repositoryWrapper.Discount.Delete(discount);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
