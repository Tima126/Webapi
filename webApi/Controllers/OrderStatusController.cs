using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public OrderStatusController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderStatus> orderStatuses = Context.OrderStatus.ToList();
            return Ok(orderStatuses);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            OrderStatus? orderStatuses = Context.OrderStatus.Where(x => x.StatusId == id).FirstOrDefault();
            if (orderStatuses == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(orderStatuses);
        }

        [HttpPost]
        public IActionResult Add(OrderStatus orderStatuses)
        {
            Context.OrderStatus.Add(orderStatuses);
            Context.SaveChanges();
            return Ok(orderStatuses);
        }


        [HttpPut]
        public IActionResult Update(OrderStatus orderStatuses)
        {
            Context.OrderStatus.Update(orderStatuses);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            OrderStatus? OrderStatus = Context.OrderStatus.Where(x => x.StatusId == id).FirstOrDefault();
            if (OrderStatus == null)
            {
                return BadRequest("Not Found");
            }
            Context.OrderStatus.Remove(OrderStatus);
            Context.SaveChanges();
            return Ok();
        }
    }
}
