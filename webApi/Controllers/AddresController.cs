using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using BusinessLogic.Services;
using Mapster;
using webApi.Contracts;
using Domain.interfaces.Service;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressesService;

        public AddressesController(IAddressService addressService)
        {
            _addressesService = addressService;
        }

        /// <summary>
        /// Получение всех адресов.
        /// </summary>
        /// <returns>Список всех адресов.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressesService.GetAll();
            return Ok(addresses);
        }

        /// <summary>
        /// Получение адреса по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор адреса.</param>
        /// <returns>Адрес с указанным идентификатором.</returns>
        /// <response code="200">Возвращает адрес.</response>
        /// <response code="404">Если адрес не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _addressesService.GetById(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        /// <summary>
        /// Создание нового адреса.
        /// </summary>
        /// <param name="address">Данные нового адреса.</param>
        /// <returns>Созданный адрес.</returns>
        /// <response code="201">Возвращает созданный адрес.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateAddress req)
        {
            var address = req.Adapt<Address>();
            await _addressesService.Create(address);
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
            var address = req.Adapt<Address>();
            await _addressesService.Update(address);

            return NoContent();
        }

        /// <summary>
        /// Удаление адреса по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор адреса.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если адрес успешно удален.</response>
        /// <response code="400">Если адрес не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _addressesService.GetById(id);

            if (address == null)
            {
                return BadRequest();
            }
            await _addressesService.Delete(id);
            return Ok();
        }
    }
}