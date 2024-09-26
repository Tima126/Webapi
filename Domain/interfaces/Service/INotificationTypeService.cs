using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces.Service
{
    public interface INotificationTypeService
    {
        Task<List<NotificationType>> GetAll();
        Task<NotificationType> GetById(int id);

        Task Create(NotificationType model);
        Task Update(NotificationType model);

        Task Delete(int id);
    }
}
