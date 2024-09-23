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

            return wishlist.First();
        }

        public async Task Create(Wishlist model)
        {
            await _repositoryWrapper.Wishlist.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(Wishlist model)
        {
            _repositoryWrapper.Wishlist.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var wishlist = await _repositoryWrapper.Wishlist
                .FindByCondition(x => x.WishlistId == id);

            _repositoryWrapper.Wishlist.Delete(wishlist.First());
            _repositoryWrapper.Save();
        }

    }
}
