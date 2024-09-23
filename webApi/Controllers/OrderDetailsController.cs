using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public OrderDetailsController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderDetail> orderStatuses = Context.OrderDetails.ToList();
            return Ok(orderStatuses);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            OrderDetail? orderDetail = Context.OrderDetails.Where(x => x.OrderDetailId == id).FirstOrDefault();
            if (orderDetail == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(orderDetail);
        }

        [HttpPost]
        public IActionResult Add(OrderDetail orderDetail)
        {
            Context.OrderDetails.Add(orderDetail);
            Context.SaveChanges();
            return Ok(orderDetail);
        }


        [HttpPut]
        public IActionResult Update(OrderDetail orderDetail)
        {
            Context.OrderDetails.Update(orderDetail);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            OrderDetail? orderDetail = Context.OrderDetails.Where(x => x.OrderDetailId == id).FirstOrDefault();
            if (orderDetail == null)
            {
                return BadRequest("Not Found");
            }
            Context.OrderDetails.Remove(orderDetail);
            Context.SaveChanges();
            return Ok();
        }
    }
}
