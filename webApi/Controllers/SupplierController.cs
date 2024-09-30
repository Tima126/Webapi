using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.interfaces.Service;
using BusinessLogic.Services;
using webApi.Contracts;
using Mapster;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplier)
        {
            _supplierService = supplier;
        }


        /// <summary>
        /// Получение всех поставщиков.
        /// </summary>
        /// <returns>Список всех поставщиков.</returns>
        /// <response code="200">Возвращает список поставщиков.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _supplierService.GetAll();
            return Ok(dis);
        }


        /// <summary>
        /// Получение поставщиков по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщиков.</param>
        /// <returns>поставщик с указанным идентификатором.</returns>
        /// <response code="200">Возвращает поставщика.</response>
        /// <response code="400">Если поставщик не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _supplierService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }
        /// <summary>
        /// Добавление нового поставщика.
        /// </summary>
        /// <param name="reviwe">Данные нового поставщика.</param>
        /// <returns>Создание поставщика.</returns>
        /// <response code="200">Возвращает созданного поставщика.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplier req)
        {
            var cat = req.Adapt<Supplier>();
            await _supplierService.Create(cat);
            return Ok();
        }

        /// <summary>
        /// Обновление существующего поставщика.
        /// </summary>
        /// <param name="disc">Данные для обновления поставщика.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если поставщик успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateSupplier disc)
        {
            var cat = disc.Adapt<Supplier>();
            await _supplierService.Update(cat);
            return NoContent();
        }


        /// <summary>
        /// Удаление поставщика по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщика.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если поставщик успешно удален.</response>
        /// <response code="400">Если поставщик не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _supplierService.GetById(id);

            if (address == null)
            {
                return BadRequest();
            }
            await _supplierService.Delete(id);
            return Ok();
        }

    }
}
