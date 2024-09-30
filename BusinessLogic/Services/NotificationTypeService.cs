using Domain.Models;
using Domain.interfaces;
using Domain.interfaces.Service;

namespace BusinessLogic.Services
{
    public class NotificationTypeService: INotificationTypeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NotificationTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }




        public async Task<List<NotificationType>> GetAll()
        {
            return await _repositoryWrapper.Notificationtype.FindAll();
        }


        public async Task<NotificationType> GetById(int id)
        {
            var notificationtype = await _repositoryWrapper.Notificationtype
                .FindByCondition(x => x.NotificationTypeId == id);

            return notificationtype.First();
        }

        public async Task Create(NotificationType model)
        {
            await _repositoryWrapper.Notificationtype.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(NotificationType model)
        {
            await _repositoryWrapper.Notificationtype.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.Notificationtype
                .FindByCondition(x => x.NotificationTypeId == id);

            await _repositoryWrapper.Notificationtype.Delete(user.First());
            await _repositoryWrapper.Save();
        }

    }
}
