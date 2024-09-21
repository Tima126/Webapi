using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class OrderStatusService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderStatusService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<OrderStatus>> GetAll()
        {
            return _repositoryWrapper.OrderStatus.FindAll().ToListAsync();
        }


        public Task<OrderStatus> GetById(int id)
        {
            var status = _repositoryWrapper.OrderStatus.FinByCondition(x => x.StatusId == id).First();
            return Task.FromResult(status);
        }

        public Task Create(OrderStatus model)
        {
            _repositoryWrapper.OrderStatus.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(OrderStatus model)
        {
            _repositoryWrapper.OrderStatus.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var status = _repositoryWrapper.OrderStatus.FinByCondition(x => x.StatusId == id).First();

            _repositoryWrapper.OrderStatus.Delete(status);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

    }
}
