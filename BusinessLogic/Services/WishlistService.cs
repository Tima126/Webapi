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
    public class WishlistService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public WishlistService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<Wishlist>> GetAll()
        {
            return _repositoryWrapper.Wishlist.FindAll().ToListAsync();
        }


        public Task<Wishlist> GetById(int id)
        {
            var wishlist = _repositoryWrapper.Wishlist.FinByCondition(x => x.WishlistId == id).First();
            return Task.FromResult(wishlist);
        }

        public Task Create(Wishlist model)
        {
            _repositoryWrapper.Wishlist.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Wishlist model)
        {
            _repositoryWrapper.Wishlist.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var wishlist = _repositoryWrapper.Wishlist.FinByCondition(x => x.WishlistId == id).First();

            _repositoryWrapper.Wishlist.Delete(wishlist);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
