using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.interfaces.Service;
using webApi.Contracts;
using Mapster;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlist)
        {
            _wishlistService = wishlist;
        }

        /// <summary>
        /// Получение всех Список желаний.
        /// </summary>
        /// <returns>Список всех желаний.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _wishlistService.GetAll();
            return Ok(addresses);
        }

        /// <summary>
        /// Получение Список желаний по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Список желаний.</param>
        /// <returns>Список желаний с указанным идентификатором.</returns>
        /// <response code="200">Возвращает Список желаний.</response>
        /// <response code="404">Если  желания не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _wishlistService.GetById(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }
        /// <summary>
        /// Создание нового желания.
        /// </summary>
        /// <param name="address">Данные нового желания.</param>
        /// <returns>Созданный Список желания.</returns>
        /// <response code="201">Возвращает созданный Список желания.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateWishlist req)
        {
            var wh = req.Adapt<Wishlist>();
            await _wishlistService.Create(wh);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего адреса.
        /// </summary>
        /// <param name="address">Данные для обновления адреса.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="204">Если адрес успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateAddress req)
        {
            var address = req.Adapt<Wishlist>();
            await _wishlistService.Update(address);

            return NoContent();
        }

        /// <summary>
        /// Удаление Список желания по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Список желания.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если Список желания успешно удален.</response>
        /// <response code="400">Если Список желания не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _wishlistService.GetById(id);

            if (address == null)
            {
                return BadRequest();
            }
            await _wishlistService.Delete(id);
            return Ok();
        }
    }
}
