using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public DiscountController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Discount> discounts = Context.Discounts.ToList();
            return Ok(discounts);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Discount? disc = Context.Discounts.Where(x => x.DiscountId == id).FirstOrDefault();
            if (disc == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(disc);
        }

        [HttpPost]
        public IActionResult Add(Discount disc)
        {
            Context.Discounts.Add(disc);
            Context.SaveChanges();
            return Ok(disc);
        }


        [HttpPut]
        public IActionResult Update(Discount disc)
        {
            Context.Discounts.Update(disc);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Discount? disc = Context.Discounts.Where(x => x.DiscountId == id).FirstOrDefault();
            if (disc == null)
            {
                return BadRequest("Not Found");
            }
            Context.Discounts.Remove(disc);
            Context.SaveChanges();
            return Ok();
        }
    }
}
