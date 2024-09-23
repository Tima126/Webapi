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
    public class NotificationTypeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NotificationTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }



        public Task<List<NotificationType>> GetAll()
        {
            return _repositoryWrapper.Notificationtype.FindAll().ToListAsync();
        }


        public Task<NotificationType> GetById(int id)
        {
            var notificationtype = _repositoryWrapper.Notificationtype.FinByCondition(x => x.NotificationTypeId == id).First();
            return Task.FromResult(notificationtype);
        }

        public Task Create(NotificationType model)
        {
            _repositoryWrapper.Notificationtype.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(NotificationType model)
        {
            _repositoryWrapper.Notificationtype.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var notificationType = _repositoryWrapper.Notificationtype.FinByCondition(x => x.NotificationTypeId == id).First();

            _repositoryWrapper.Notificationtype.Delete(notificationType);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
