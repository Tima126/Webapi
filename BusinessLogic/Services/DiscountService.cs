using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class DiscountService: IDicountServices
    {
        private IRepositoryWrapper _repositoryWrapper;

        public DiscountService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<Discount>> GetAll()
        {
            return await _repositoryWrapper.Discount.FindAll();
        }


        public async Task<Discount> GetById(int id)
        {
            var discount = await _repositoryWrapper.Discount
                .FindByCondition(x => x.DiscountId == id);

            return discount.First();
        }

        public async Task Create(Discount model)
        {
            await _repositoryWrapper.Discount.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Discount model)
        {
            await _repositoryWrapper.Discount.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var discount = await _repositoryWrapper.Discount
                .FindByCondition(x => x.DiscountId == id);

            await _repositoryWrapper.Discount.Delete(discount.First());
            await _repositoryWrapper.Save();
        }

    }
}
