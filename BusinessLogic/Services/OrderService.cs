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
    public class OrderService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Order>> GetAll()
        {
            return _repositoryWrapper.Order.FindAll().ToListAsync();
        }


        public Task<Order> GetById(int id)
        {
            var order = _repositoryWrapper.Order.FinByCondition(x => x.OrderId == id).First();
            return Task.FromResult(order);
        }

        public Task Create(Order model)
        {
            _repositoryWrapper.Order.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Order model)
        {
            _repositoryWrapper.Order.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var order = _repositoryWrapper.Order.FinByCondition(x => x.OrderId == id).First();

            _repositoryWrapper.Order.Delete(order);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
