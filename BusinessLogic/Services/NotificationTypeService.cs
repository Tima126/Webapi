using Domain.Models;
using Domain.interfaces;
using Domain.interfaces.Service;
using FluentValidation;
using BusinessLogic.Validation;

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
            
            if (!notificationtype.Any())
            {
                throw new InvalidOperationException($"Notificationtype with id {id} not found.");
            }
            return notificationtype.First();
        }

        public async Task Create(NotificationType model)
        {
            var validator = new NotificationTypeValid();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Notificationtype.Create(model);
            await _repositoryWrapper.Save();
        }



        public async Task Update(NotificationType model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var validator = new NotificationTypeValid();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                string errorMessageString = string.Join(", ", errorMessages);
                throw new ValidationException(errorMessageString);
            }
            await _repositoryWrapper.Notificationtype.Update(model);
            await _repositoryWrapper.Save();
        }


        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.Notificationtype
                .FindByCondition(x => x.NotificationTypeId == id);

            if (!user.Any())
            {
                throw new InvalidOperationException($"NotificationType with id {id} not found");
            }
            await _repositoryWrapper.Notificationtype.Delete(user.First());
            await _repositoryWrapper.Save();
        }

    }
}
