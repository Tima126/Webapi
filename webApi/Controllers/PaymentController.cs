using BusinessLogic.Services;
using Domain.interfaces.Service;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApi.Contracts;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Получение всех платежей.
        /// </summary>
        /// <returns>Список всех платежей.</returns>
        /// <response code="200">Возвращает список платежей.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _paymentService.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение платежа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор платежа.</param>
        /// <returns>Платеж с указанным идентификатором.</returns>
        /// <response code="200">Возвращает платеж.</response>
        /// <response code="400">Если платеж не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _paymentService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }
        /// <summary>
        /// Добавление нового платежа.
        /// </summary>
        /// <param name="payment">Данные нового платежа.</param>
        /// <returns>Созданный платеж.</returns>
        /// <response code="200">Возвращает созданный платеж.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreatePayment req)
        {
            var cat = req.Adapt<Payment>();
            await _paymentService.Create(cat);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего платежа.
        /// </summary>
        /// <param name="payment">Данные для обновления платежа.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если платеж успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreatePayment disc)
        {
            var cat = disc.Adapt<Payment>();
            await _paymentService.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление платежа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор платежа.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если платеж успешно удален.</response>
        /// <response code="400">Если платеж не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _paymentService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _paymentService.Delete(id);
            return Ok();
        }
    }
}