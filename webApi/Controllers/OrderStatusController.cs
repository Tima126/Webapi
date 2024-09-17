using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ModelsCons_.Models;

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
            List<OrderStatus> orderStatuses = Context.OrderStatuses.ToList();
            return Ok(orderStatuses);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            OrderStatus? orderStatuses = Context.OrderStatuses.Where(x => x.StatusId == id).FirstOrDefault();
            if (orderStatuses == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(orderStatuses);
        }

        [HttpPost]
        public IActionResult Add(OrderStatus orderStatuses)
        {
            Context.OrderStatuses.Add(orderStatuses);
            Context.SaveChanges();
            return Ok(orderStatuses);
        }


        [HttpPut]
        public IActionResult Update(OrderStatus orderStatuses)
        {
            Context.OrderStatuses.Update(orderStatuses);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            OrderStatus? OrderStatus = Context.OrderStatuses.Where(x => x.StatusId == id).FirstOrDefault();
            if (OrderStatus == null)
            {
                return BadRequest("Not Found");
            }
            Context.OrderStatuses.Remove(OrderStatus);
            Context.SaveChanges();
            return Ok();
        }
    }
}
