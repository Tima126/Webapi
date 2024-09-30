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
    public class NotificationTypeController : ControllerBase
    {
        private readonly INotificationTypeService _notificationType;

        public NotificationTypeController(INotificationTypeService notificationType)
        {
            _notificationType = notificationType;
        }
        /// <summary>
        /// Получение всех типов уведомлений.
        /// </summary>
        /// <returns>Список всех типов уведомлений.</returns>
        /// <response code="200">Возвращает список типов уведомлений.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dis = await _notificationType.GetAll();
            return Ok(dis);
        }

        /// <summary>
        /// Получение типа уведомления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор типа уведомления.</param>
        /// <returns>Тип уведомления с указанным идентификатором.</returns>
        /// <response code="200">Возвращает тип уведомления.</response>
        /// <response code="400">Если тип уведомления не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            var cat = await _notificationType.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }


        /// <summary>
        /// Добавление нового типа уведомления.
        /// </summary>
        /// <param name="notificationType">Данные нового типа уведомления.</param>
        /// <returns>Созданный тип уведомления.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationType req)
        {
            var cat = req.Adapt<NotificationType>();
            await _notificationType.Create(cat);
            return Ok();
        }
        /// <summary>
        /// Обновление существующего типа уведомления.
        /// </summary>
        /// <param name="notificationType">Данные для обновления типа уведомления.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если тип уведомления успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateNotificationType disc)
        {
            var cat = disc.Adapt<NotificationType>();
            await _notificationType.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление типа уведомления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор типа уведомления.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если тип уведомления успешно удален.</response>
        /// <response code="400">Если тип уведомления не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _notificationType.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _notificationType.Delete(id);
            return Ok();
        }
    }
}