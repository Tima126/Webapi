using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public async Task<List<User>> GetAll()
        {
            return await _repositoryWrapper.User.FindAll();
        }


        public async Task<User> GetById(int id)
        {
            var user = await _repositoryWrapper.User
                .FindByCondition(x => x.UserId == id);

            if (!user.Any())
            {
                throw new InvalidOperationException($"User with id {id} not found.");
            }

            return user.First();
        }

        public async Task Create(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                throw new ArgumentException(nameof(model.FirstName));
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                throw new ArgumentException(nameof(model.LastName));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException(nameof(model.Email));
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                throw new ArgumentException(nameof(model.PhoneNumber));
            }

            if (model.AddressId <= 0)
            {
                throw new ArgumentException(nameof(model.AddressId));
            }

            await _repositoryWrapper.User.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            await _repositoryWrapper.User.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.User
                .FindByCondition(x => x.UserId == id);

            if (!user.Any())
            {
                throw new InvalidOperationException($"User with id {id} not found");
            }

            await _repositoryWrapper.User.Delete(user.First());
            await _repositoryWrapper.Save();
        }



    }
}
