using Domain.Models;
using Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.interfaces.Service;

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

            return notification.First();
        }

        public async Task Create(Notification model)
        {
            await _repositoryWrapper.Notification.Create(model);
            _repositoryWrapper.Save();
        }



        public async Task Update(Notification model)
        {
            _repositoryWrapper.Notification.Update(model);
            _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var notification = await _repositoryWrapper.Notification
                .FindByCondition(x => x.NotificationId == id);

            _repositoryWrapper.Notification.Delete(notification.First());
            _repositoryWrapper.Save();
        }

    }
}
