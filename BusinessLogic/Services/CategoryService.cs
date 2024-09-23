using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

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

            return category.First();
        }

        public async Task Create(Category model)
        {
            await _repositoryWrapper.Category.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(Category model)
        {
            _repositoryWrapper.Category.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var category = await _repositoryWrapper.Category
                .FindByCondition(x => x.CategoryId == id);

            _repositoryWrapper.Category.Delete(category.First());
            _repositoryWrapper.Save();
        }

    }
}
