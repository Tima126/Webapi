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

            return orderStatus.First();
        }

        public async Task Create(OrderStatus model)
        {
            await _repositoryWrapper.OrderStatus.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(OrderStatus model)
        {
            _repositoryWrapper.OrderStatus.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var orderStatus = await _repositoryWrapper.OrderStatus
                .FindByCondition(x => x.StatusId == id);

            _repositoryWrapper.OrderStatus.Delete(orderStatus.First());
            _repositoryWrapper.Save();
        }

    }
}
