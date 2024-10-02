﻿using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using Domain.interfaces.Service;
using System.Net;
using BusinessLogic.Validation;
using FluentValidation;

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
            if (!discount.Any())
            {
                throw new InvalidOperationException($"Discount with id {id} not found");
            }
            return discount.First();
        }

        public async Task Create(Discount model)
        {


            var validator = new DiscountValid();
            var validattorResult = validator.Validate(model);

            if (!validattorResult.IsValid)
            {
                var errorMassages = validattorResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMassagesString = string.Join(", ", errorMassages);
                throw new ValidationException(errorMassagesString);
            }

            await _repositoryWrapper.Discount.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Discount model)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new DiscountValid();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }


            await _repositoryWrapper.Discount.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var discount = await _repositoryWrapper.Discount
                .FindByCondition(x => x.DiscountId == id);
            if (!discount.Any())
            {
                throw new InvalidOperationException($"Discount with id {id} not found");
            }
            await _repositoryWrapper.Discount.Delete(discount.First());
            await _repositoryWrapper.Save();
        }

    }
}
