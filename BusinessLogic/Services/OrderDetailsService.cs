using Domain.Models;
using Domain.interfaces;
;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class OrderDetailsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public OrderDetailsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<OrderDetail>> GetAll()
        {
            return _repositoryWrapper.OrderDetail.FindAll().ToListAsync();
        }


        public Task<OrderDetail> GetById(int id)
        {
            var orderDetail = _repositoryWrapper.OrderDetail.FinByCondition(x => x.OrderDetailId == id).First();
            return Task.FromResult(orderDetail);
        }

        public Task Create(OrderDetail model)
        {
            _repositoryWrapper.OrderDetail.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(OrderDetail model)
        {
            _repositoryWrapper.OrderDetail.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var orderDetail = _repositoryWrapper.OrderDetail.FinByCondition(x => x.OrderDetailId == id).First();

            _repositoryWrapper.OrderDetail.Delete(orderDetail);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
