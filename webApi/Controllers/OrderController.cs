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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;

        public OrderController(IOrderService orderService)
        {
            _orderservice = orderService;
        }

        /// <summary>
        /// Получение всех заказов.
        /// </summary>
        /// <returns>Список всех заказов.</returns>
        /// <response code="200">Возвращает список заказов.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _orderservice.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>Заказ с указанным идентификатором.</returns>
        /// <response code="200">Возвращает заказ.</response>
        /// <response code="400">Если заказ не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _orderservice.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление нового заказа.
        /// </summary>
        /// <param name="order">Данные нового заказа.</param>
        /// <returns>Созданный заказ.</returns>
        /// <response code="200">Возвращает созданный заказ.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrder req)
        {
            var cat = req.Adapt<Order>();
            await _orderservice.Create(cat);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего заказа.
        /// </summary>
        /// <param name="order">Данные для обновления заказа.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если заказ успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateOrder disc)
        {
            var cat = disc.Adapt<Order>();
            await _orderservice.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если заказ успешно удален.</response>
        /// <response code="400">Если заказ не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _orderservice.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _orderservice.Delete(id);
            return Ok();
        }
    }
}