using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        public FlowersStoreContext Context { get; }
        public AddressesController(FlowersStoreContext context)
        {
            Context = context;
        }

  
        [HttpGet]
        public IActionResult GetAll()
        {
            var addresses = Context.Addresses.ToList();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var address = Context.Addresses.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }


        [HttpPost]
        public IActionResult Create(Address address)
        {
            Context.Addresses.Add(address);
            Context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = address.AddressId }, address);
        }

        [HttpPut]
        public IActionResult Update(Address address)
        {
            
            Context.Entry(address).State = EntityState.Modified;
            Context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            Address? addres = Context.Addresses.Where(x => x.AddressId == id).FirstOrDefault();

            if (addres == null)
            {
                return BadRequest(User);

            }
            Context.Addresses.Remove(addres);
            Context.SaveChanges();
            return Ok();
        }


    }
}