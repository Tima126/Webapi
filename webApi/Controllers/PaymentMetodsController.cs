using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ModelsCons_.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMetodsController : ControllerBase
    {

        public FlowersStoreContext Context { get; }

        public PaymentMetodsController(FlowersStoreContext context)
        {
            Context = context;
        }




        [HttpGet]
        public IActionResult GetAll()
        {
            List<PaymentMethod> paymentMethods = Context.PaymentMethods.ToList();
            return Ok(paymentMethods);
        }


        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            PaymentMethod? upaymentMethodsser = Context.PaymentMethods.Where(x => x.PaymentMethodId == id).FirstOrDefault();
            if (upaymentMethodsser == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(upaymentMethodsser);
        }

        [HttpPost]
        public IActionResult Add(PaymentMethod upaymentMethodsser)
        {
            Context.PaymentMethods.Add(upaymentMethodsser);
            Context.SaveChanges();
            return Ok(upaymentMethodsser);
        }


        [HttpPut]
        public IActionResult Update(PaymentMethod upaymentMethodsser)
        {
            Context.PaymentMethods.Update(upaymentMethodsser);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            PaymentMethod? payment = Context.PaymentMethods.Where(x => x.PaymentMethodId == id).FirstOrDefault();
            if (payment == null)
            {
                return BadRequest("Not Found");
            }
            Context.PaymentMethods.Remove(payment);
            Context.SaveChanges();
            return Ok();
        }


    }
}
