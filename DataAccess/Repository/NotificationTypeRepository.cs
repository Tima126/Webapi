using Domain.interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class NotificationTypeRepository : RepositoryBase<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(FlowersStoreContext repositoriContext)
: base(repositoriContext)
        {

        }
    }
}
