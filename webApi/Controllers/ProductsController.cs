using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {



        public FlowersStoreContext Context { get; }


        public ProductsController (FlowersStoreContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = Context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Product? products = Context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            if ( products== null)
            {
                return BadRequest("Not Found");
            }
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            Context.Products.Add(product);
            Context.SaveChanges();
            return Ok(product);
        }


        [HttpPut]
        public IActionResult Update(Product product)
        {
            Context.Products.Update(product);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Product? product = Context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            if (product == null)
            {
                return BadRequest("Not Found");
            }
            Context.Products.Remove(product);
            Context.SaveChanges();
            return Ok();
        }



    }
}
