using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public NotificationTypeController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<NotificationType> notifications = Context.NotificationTypes.ToList();
            return Ok(notifications);

        }

        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            NotificationType? notifications = Context.NotificationTypes.Where(x => x.NotificationTypeId == id).FirstOrDefault();
            if (notifications == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult Add(NotificationType notification)
        {
            Context.NotificationTypes.Add(notification);
            Context.SaveChanges();
            return Ok(notification);
        }

        [HttpPut]
        public IActionResult Update(NotificationType notification)
        {
            Context.NotificationTypes.Update(notification);
            Context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            NotificationType? notification = Context.NotificationTypes.Where(x => x.NotificationTypeId == id).FirstOrDefault();
            if (notification == null)
            {
                return BadRequest("Not Found");
            }
            Context.NotificationTypes.Remove(notification);
            Context.SaveChanges();
            return Ok();
        }
    }
}
