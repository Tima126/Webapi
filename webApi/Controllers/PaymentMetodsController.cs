using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using BusinessLogic.Services;
using Domain.interfaces.Service;
using webApi.Contracts;
using Mapster;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMetodsController : ControllerBase
    {

        private readonly IPaymentMethodService _paymentmethodService;

        public PaymentMetodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentmethodService = paymentMethodService;
        }



        /// <summary>
        /// Получение всех методов платежей.
        /// </summary>
        /// <returns>Список всех методов платежей.</returns>
        /// <response code="200">Возвращает список методов платежей.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _paymentmethodService.GetAll();
            return Ok(dis);
        }



        /// <summary>
        /// Получение метода платежа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор метода платежа.</param>
        /// <returns> Метод Платежа с указанным идентификатором.</returns>
        /// <response code="200">Возвращает метод платежа.</response>
        /// <response code="400">Если метод платеж не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _paymentmethodService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }




        /// <summary>
        /// Добавление нового метода платежа.
        /// </summary>
        /// <param name="payment">Данные нового метод платежа.</param>
        /// <returns>Созданный платеж.</returns>
        /// <response code="200">Возвращает созданный метод платежа.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentMethod req)
        {
            var cat = req.Adapt<PaymentMethod>();
            await _paymentmethodService.Create(cat);
            return Ok();
        }


        /// <summary>
        /// Обновление существующего метода платежа.
        /// </summary>
        /// <param name="payment">Данные для обновления метода платежа.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если метод платежа успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreatePaymentMethod disc)
        {
            var cat = disc.Adapt<PaymentMethod>();
            await _paymentmethodService.Update(cat);
            return NoContent();
        }


        /// <summary>
        /// Удаление метода платежа по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор метода платежа.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если метода платеж успешно удален.</response>
        /// <response code="400">Если метода платеж не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _paymentmethodService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _paymentmethodService.Delete(id);
            return Ok();
        }

    }
}
