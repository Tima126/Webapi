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
    public class ReviweController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviweController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }




        /// <summary>
        /// Получение всех отзывов.
        /// </summary>
        /// <returns>Список всех отзывов.</returns>
        /// <response code="200">Возвращает список отзывов.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _reviewService.GetAll();
            return Ok(dis);
        }


        /// <summary>
        /// Получение отзовов по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор отзовов.</param>
        /// <returns>Скидка с указанным идентификатором.</returns>
        /// <response code="200">Возвращает отзова.</response>
        /// <response code="400">Если отзова не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _reviewService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление новой скидки.
        /// </summary>
        /// <param name="reviwe">Данные нового отзова.</param>
        /// <returns>Созданная скидка.</returns>
        /// <response code="200">Возвращает созданную скидку.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviwe req)
        {
            var cat = req.Adapt<Review>();
            await _reviewService.Create(cat);
            return Ok();
        }


        /// <summary>
        /// Обновление существующего отзова.
        /// </summary>
        /// <param name="disc">Данные для обновления отзова.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если отзов успешно обновлена.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateReviwe disc)
        {
            var cat = disc.Adapt<Review>();
            await _reviewService.Update(cat);
            return NoContent();
        }


        /// Удаление отзовов по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор отзова.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если отзов успешно удалена.</response>
        /// <response code="400">Если отзов не найдена.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _reviewService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _reviewService.Delete(id);
            return Ok();
        }
    }
}
