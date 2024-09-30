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
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatus;

        public OrderStatusController(IOrderStatusService orderStatus)
        {
            _orderStatus = orderStatus;
        }


        /// <summary>
        /// Получение всех статусов заказов.
        /// </summary>
        /// <returns>Список всех статусов заказов.</returns>
        /// <response code="200">Возвращает список статусов заказов.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _orderStatus.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение статуса заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор статуса заказа.</param>
        /// <returns>Статус заказа с указанным идентификатором.</returns>
        /// <response code="200">Возвращает статус заказа.</response>
        /// <response code="400">Если статус заказа не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _orderStatus.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление нового статуса заказа.
        /// </summary>
        /// <param name="orderStatus">Данные нового статуса заказа.</param>
        /// <returns>Созданный статус заказа.</returns>
        /// <response code="200">Возвращает созданный статус заказа.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderStatus req)
        {
            var cat = req.Adapt<OrderStatus>();
            await _orderStatus.Create(cat);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего статуса заказа.
        /// </summary>
        /// <param name="orderStatus">Данные для обновления статуса заказа.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если статус заказа успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateOrderStatus disc)
        {
            var cat = disc.Adapt<OrderStatus>();
            await _orderStatus.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление статуса заказа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор статуса заказа.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если статус заказа успешно удален.</response>
        /// <response code="400">Если статус заказа не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _orderStatus.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _orderStatus.Delete(id);
            return Ok();
        }
    }
}