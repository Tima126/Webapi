using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class OrderDetailsService:IOrderDetailsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderDetailsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public async Task<List<OrderDetail>> GetAll()
        {
            return await _repositoryWrapper.OrderDetail.FindAll();
        }


        public async Task<OrderDetail> GetById(int id)
        {
            var user = await _repositoryWrapper.OrderDetail
                .FindByCondition(x => x.OrderDetailId == id);

            return user.First();
        }

        public async Task Create(OrderDetail model)
        {
            await _repositoryWrapper.OrderDetail.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(OrderDetail model)
        {
            _repositoryWrapper.OrderDetail.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var orderDetail = await _repositoryWrapper.OrderDetail
                .FindByCondition(x => x.OrderDetailId == id);

            _repositoryWrapper.OrderDetail.Delete(orderDetail.First());
            _repositoryWrapper.Save();
        }

    }
}
