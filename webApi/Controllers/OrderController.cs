using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {



        public FlowersStoreContext Context { get; }

        public OrderController(FlowersStoreContext context)
        {
            Context = context;
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            List<Order> order = Context.Orders.ToList();
            return Ok(order);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Order? order = Context.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            if (order == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Add(Order order)
        {
            Context.Orders.Add(order);
            Context.SaveChanges();
            return Ok(order);
        }


        [HttpPut]
        public IActionResult Update(Order order)
        {
            Context.Orders.Update(order);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Order? order = Context.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            if (order == null)
            {
                return BadRequest("Not Found");
            }
            Context.Orders.Remove(order);
            Context.SaveChanges();
            return Ok();
        }





    }
}
