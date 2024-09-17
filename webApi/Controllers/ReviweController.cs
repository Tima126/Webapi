using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ModelsCons_.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviweController : ControllerBase
    {




        public FlowersStoreContext Context { get; }

        public ReviweController(FlowersStoreContext context)
        {
            Context = context;
        }




        [HttpGet]
        public IActionResult GetAll()
        {
            List<Review> reviwes = Context.Reviews.ToList();
            return Ok(reviwes);
        }


        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Review? reviwes = Context.Reviews.Where(x => x.ReviewId == id).FirstOrDefault();
            if (reviwes == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(reviwes);
        }

        [HttpPost]
        public IActionResult Add(Review reviwes)
        {
            Context.Reviews.Add(reviwes);
            Context.SaveChanges();
            return Ok(reviwes);
        }


        [HttpPut]
        public IActionResult Update(Review reviwes)
        {
            Context.Reviews.Update(reviwes);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Review? payment = Context.Reviews.Where(x => x.ReviewId == id).FirstOrDefault();
            if (payment == null)
            {
                return BadRequest("Not Found");
            }
            Context.Reviews.Remove(payment);
            Context.SaveChanges();
            return Ok();
        }
    }
}
