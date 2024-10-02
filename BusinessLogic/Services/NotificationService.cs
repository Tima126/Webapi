using Domain.Models;
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
    public class NotificationService:INotificationService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NotificationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public async Task<List<Notification>> GetAll()
        {
            return await _repositoryWrapper.Notification.FindAll();
        }


        public async Task<Notification> GetById(int id)
        {
            var notification = await _repositoryWrapper.Notification
                .FindByCondition(x => x.NotificationId == id);
            if (!notification.Any())
            {
                throw new InvalidOperationException($"Notification with id {id} not found");
            }
            return notification.First();
        }

        public async Task Create(Notification model)
        {
            var validator = new NotificationValid();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessagesString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessagesString);
            }

            await _repositoryWrapper.Notification.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(Notification model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new NotificationValid();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Notification.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var notification = await _repositoryWrapper.Notification
                .FindByCondition(x => x.NotificationId == id);
            if (!notification.Any())
            {
                throw new InvalidOperationException($"Notification with id {id} not found");
            }
            await _repositoryWrapper.Notification.Delete(notification.First());
            await _repositoryWrapper.Save();
        }


    }
}
