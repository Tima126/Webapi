using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public WishlistController(FlowersStoreContext context)
        {
            Context = context;
        }




        [HttpGet]
        public IActionResult GetAll()
        {
            List<Wishlist> wishlists = Context.Wishlists.ToList();
            return Ok(wishlists);
        }


        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Wishlist? wishlist = Context.Wishlists.Where(x => x.WishlistId == id).FirstOrDefault();
            if (wishlist == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(wishlist);
        }

        [HttpPost]
        public IActionResult Add(Wishlist wishlist)
        {
            Context.Wishlists.Add(wishlist);
            Context.SaveChanges();
            return Ok(wishlist);
        }


        [HttpPut]
        public IActionResult Update(Wishlist wishlist)
        {
            Context.Wishlists.Update(wishlist);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Wishlist? payment = Context.Wishlists.Where(x => x.WishlistId == id).FirstOrDefault();
            if (payment == null)
            {
                return BadRequest("Not Found");
            }
            Context.Wishlists.Remove(payment);
            Context.SaveChanges();
            return Ok();
        }
    }
}
