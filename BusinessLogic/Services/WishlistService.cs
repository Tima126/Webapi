﻿using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;
using BusinessLogic.Validation;
using FluentValidation;

namespace BusinessLogic.Services
{
    public class WishlistService:IWishlistService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public WishlistService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<Wishlist>> GetAll()
        {
            return await _repositoryWrapper.Wishlist.FindAll();
        }


        public async Task<Wishlist> GetById(int id)
        {
            var wishlist = await _repositoryWrapper.Wishlist
                .FindByCondition(x => x.WishlistId == id);
            if (!wishlist.Any())
            {
                throw new InvalidOperationException($"Wishlist with id {id} not found.");
            }
            return wishlist.First();
        }

        public async Task Create(Wishlist model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var validator = new WishlistValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Wishlist.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Wishlist model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new WishlistValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Wishlist.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var wishlist = await _repositoryWrapper.Wishlist
                .FindByCondition(x => x.WishlistId == id);
            if (!wishlist.Any())
            {
                throw new InvalidOperationException($"Wishlist with id {id} not found");
            }
            await _repositoryWrapper.Wishlist.Delete(wishlist.First());
            await _repositoryWrapper.Save();
        }

    }
}
