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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailsService;

        public OrderDetailsController(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        /// <summary>
        /// Получение всех деталей заказов.
        /// </summary>
        /// <returns>Список всех деталей заказов.</returns>
        /// <response code="200">Возвращает список деталей заказов.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _orderDetailsService.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение детали заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор детали заказа.</param>
        /// <returns>Деталь заказа с указанным идентификатором.</returns>
        /// <response code="200">Возвращает деталь заказа.</response>
        /// <response code="400">Если деталь заказа не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _orderDetailsService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление новой детали заказа.
        /// </summary>
        /// <param name="orderDetail">Данные новой детали заказа.</param>
        /// <returns>Созданная деталь заказа.</returns>
        /// <response code="200">Возвращает созданную деталь заказа.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDitail req)
        {
            var cat = req.Adapt<OrderDetail>();
            await _orderDetailsService.Create(cat);
            return Ok();
        }

        /// <summary>
        /// Обновление существующей детали заказа.
        /// </summary>
        /// <param name="orderDetail">Данные для обновления детали заказа.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если деталь заказа успешно обновлена.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateOrderDitail disc)
        {
            var cat = disc.Adapt<OrderDetail>();
            await _orderDetailsService.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление детали заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор детали заказа.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если деталь заказа успешно удалена.</response>
        /// <response code="400">Если деталь заказа не найдена.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _orderDetailsService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _orderDetailsService.Delete(id);
            return Ok();
        }
    }
}