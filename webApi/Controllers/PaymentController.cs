using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {


        public FlowersStoreContext Context { get; }

        public PaymentController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Payment> orderStatuses = Context.Payments.ToList();
            return Ok(orderStatuses);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Payment? payment = Context.Payments.Where(x => x.PaymentId == id).FirstOrDefault();
            if (payment == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(payment);
        }

        [HttpPost]
        public IActionResult Add(Payment payment)
        {
            Context.Payments.Add(payment);
            Context.SaveChanges();
            return Ok(payment);
        }


        [HttpPut]
        public IActionResult Update(Payment payment)
        {
            Context.Payments.Update(payment);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Payment? pyment = Context.Payments.Where(x => x.PaymentId == id).FirstOrDefault();
            if (pyment == null)
            {
                return BadRequest("Not Found");
            }
            Context.Payments.Remove(pyment);
            Context.SaveChanges();
            return Ok();
        }
    }
}
