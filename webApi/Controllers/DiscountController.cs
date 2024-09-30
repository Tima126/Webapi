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
    public class DiscountController : ControllerBase
    {
        private readonly IDicountServices _discountService;

        public DiscountController(IDicountServices discountService)
        {
            _discountService = discountService;
        }

        /// <summary>
        /// Получение всех скидок.
        /// </summary>
        /// <returns>Список всех скидок.</returns>
        /// <response code="200">Возвращает список скидок.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _discountService.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение скидки по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор скидки.</param>
        /// <returns>Скидка с указанным идентификатором.</returns>
        /// <response code="200">Возвращает скидку.</response>
        /// <response code="400">Если скидка не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _discountService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление новой скидки.
        /// </summary>
        /// <param name="disc">Данные новой скидки.</param>
        /// <returns>Созданная скидка.</returns>
        /// <response code="200">Возвращает созданную скидку.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscount req)
        {
            var cat = req.Adapt<Discount>();
            await _discountService.Create(cat);
            return Ok();
        }
        /// <summary>
        /// Обновление существующей скидки.
        /// </summary>
        /// <param name="disc">Данные для обновления скидки.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если скидка успешно обновлена.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateDiscount disc)
        {
            var cat = disc.Adapt<Discount>();
            await _discountService.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление скидки по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор скидки.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если скидка успешно удалена.</response>
        /// <response code="400">Если скидка не найдена.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _discountService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _discountService.Delete(id);
            return Ok();
        }
    }
}