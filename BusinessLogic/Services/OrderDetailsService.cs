using Domain.Models;
using Domain.interfaces;
using Domain.interfaces.Service;
using FluentValidation;
using BusinessLogic.Validation;

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
            var orderdetails = await _repositoryWrapper.OrderDetail
                .FindByCondition(x => x.OrderDetailId == id);
            if (!orderdetails.Any())
            {
                throw new InvalidOperationException($"OrderDetail with id {id} not found.");
            }
            return orderdetails.First();
        }

        public async Task Create(OrderDetail model)
        {
            var validator = new OrderDetailsValid();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }
            await _repositoryWrapper.OrderDetail.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(OrderDetail model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new OrderDetailsValid();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }
            await _repositoryWrapper.OrderDetail.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var orderDetail = await _repositoryWrapper.OrderDetail
                .FindByCondition(x => x.OrderDetailId == id);
            if (!orderDetail.Any())
            {
                throw new InvalidOperationException($"OrderDetail with id {id} not found");
            }
            await _repositoryWrapper.OrderDetail.Delete(orderDetail.First());
            await _repositoryWrapper.Save();
        }

    }
}
