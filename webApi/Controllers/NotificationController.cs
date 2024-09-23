using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public NotificationController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Notification> notifications = Context.Notifications.ToList();
            return Ok(notifications);

        }

        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Notification? notifications = Context.Notifications.Where(x => x.NotificationId == id).FirstOrDefault();
            if (notifications == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult Add(Notification notification)
        {
            Context.Notifications.Add(notification);
            Context.SaveChanges();
            return Ok(notification);
        }

        [HttpPut]
        public IActionResult Update(Notification notification)
        {
            Context.Notifications.Update(notification);
            Context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Notification? notification = Context.Notifications.Where(x => x.NotificationId == id).FirstOrDefault();
            if (notification == null)
            {
                return BadRequest("Not Found");
            }
            Context.Notifications.Remove(notification);
            Context.SaveChanges();
            return Ok();
        }
    }
}
