using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriController : ControllerBase
    {

        public FlowersStoreContext Context { get; }

        public CategoriController(FlowersStoreContext context)
        {
            Context = context;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> cate = Context.Categories.ToList();
            return Ok(cate);

        }



        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            Category? cate = Context.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            if (cate == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(cate);
        }

        [HttpPost]
        public IActionResult Add(Category cate)
        {
            Context.Categories.Add(cate);
            Context.SaveChanges();
            return Ok(cate);
        }


        [HttpPut]
        public IActionResult Update(Category cate)
        {
            Context.Categories.Update(cate);
            Context.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Category? cate = Context.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            if (cate == null)
            {
                return BadRequest("Not Found");
            }
            Context.Categories.Remove(cate);
            Context.SaveChanges();
            return Ok();
        }

    }
}
