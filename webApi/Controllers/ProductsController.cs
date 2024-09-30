using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using BusinessLogic.Services;
using webApi.Contracts;
using Mapster;
using Domain.interfaces.Service;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {



        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Получение всех продуктов.
        /// </summary>
        /// <returns>Список всех продуктов.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }


        /// <summary>
        /// Получение продуктов по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продуктов.</param>
        /// <returns>продукт с указанным идентификатором.</returns>
        /// <response code="200">Возвращает продукт.</response>
        /// <response code="404">Если адрес не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Создание нового продукта.
        /// </summary>
        /// <param name="product">Данные нового продукта.</param>
        /// <returns>Созданный продукт.</returns>
        /// <response code="201">Возвращает созданный продукт.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateProducts req)
        {
            var address = req.Adapt<Product>();
            await _productService.Create(address);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего продукта.
        /// </summary>
        /// <param name="product">Данные для обновления продукта.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="204">Если продукт успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateProducts req)
        {
            var product = req.Adapt<Product>();
            await _productService.Update(product);

            return NoContent();
        }



        /// <summary>
        /// Удаление продукта по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если продукт успешно удален.</response>
        /// <response code="400">Если продукт не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _productService.GetById(id);

            if (address == null)
            {
                return BadRequest();
            }
            await _productService.Delete(id);
            return Ok();
        }


    }
}
