using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;
using System.Net;
using BusinessLogic.Validation;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace BusinessLogic.Services
{
    public class CategoryService:ICategoryService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CategoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<List<Category>> GetAll()
        {
            return await _repositoryWrapper.Category.FindAll();
        }


        public async Task<Category> GetById(int id)
        {
            var category = await _repositoryWrapper.Category
                .FindByCondition(x => x.CategoryId == id);
            if (!category.Any())
            {
                throw new InvalidOperationException($"Category with id {id} not found");
            }

            return category.First();
        }

        public async Task Create(Category model)
        {
            var validator = new CategoryValid();
            var validattorResult = validator.Validate(model);

            if (!validattorResult.IsValid)
            {
                var errorMassages = validattorResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMassagesString = string.Join(", ", errorMassages);
                throw new FluentValidation.ValidationException(errorMassagesString);
            }
            await _repositoryWrapper.Category.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Category model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new CategoryValid();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new FluentValidation.ValidationException(errorMessageString);
            }




            await _repositoryWrapper.Category.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var category = await _repositoryWrapper.Category
                .FindByCondition(x => x.CategoryId == id);
            if (!category.Any())
            {
                throw new InvalidOperationException($"Category with id {id} not found");
            }
            await _repositoryWrapper.Category.Delete(category.First());
            await _repositoryWrapper.Save();
        }

    }
}
