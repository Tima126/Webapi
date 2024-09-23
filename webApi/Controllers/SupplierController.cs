using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        public FlowersStoreContext Context { get; }

        public SupplierController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Supplier> sup = Context.Suppliers.ToList();
            return Ok(sup);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Supplier? sup = Context.Suppliers.Where(x => x.SupplierId == id).FirstOrDefault();
            if (sup == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(sup);
        }

        [HttpPost]
        public IActionResult Add(Supplier sup)
        {
            Context.Suppliers.Add(sup);
            Context.SaveChanges();
            return Ok(sup);
        }


        [HttpPut]
        public IActionResult Update(Supplier sup)
        {
            Context.Suppliers.Update(sup);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Supplier? sup = Context.Suppliers.Where(x => x.SupplierId == id).FirstOrDefault();
            if (sup == null)
            {
                return BadRequest("Not Found");
            }
            Context.Suppliers.Remove(sup);
            Context.SaveChanges();
            return Ok();
        }


    }
}
