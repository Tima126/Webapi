using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces;
using Domain.interfaces.Service;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

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
            var validator = new UserValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                var errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
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
