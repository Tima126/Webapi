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
    public class CategoryService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CategoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Category>> GetAll()
        {
            return _repositoryWrapper.Category.FindAll().ToListAsync();
        }


        public Task<Category> GetById(int id)
        {
            var category = _repositoryWrapper.Category.FinByCondition(x => x.CategoryId == id).First();
            return Task.FromResult(category);
        }

        public Task Create(Category model)
        {
            _repositoryWrapper.Category.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Category model)
        {
            _repositoryWrapper.Category.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var category = _repositoryWrapper.Category.FinByCondition(x => x.CategoryId == id).First();

            _repositoryWrapper.Category.Delete(category);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
