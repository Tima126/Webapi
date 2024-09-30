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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Получение всех уведомлений.
        /// </summary>
        /// <returns>Список всех уведомлений.</returns>
        /// <response code="200">Возвращает список уведомлений.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categor = await _notificationService.GetAll();
            return Ok(categor);
        }

        /// <summary>
        /// Получение уведомления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор уведомления.</param>
        /// <returns>Уведомление с указанным идентификатором.</returns>
        /// <response code="200">Возвращает уведомление.</response>
        /// <response code="400">Если уведомление не найдено.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _notificationService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление нового уведомления.
        /// </summary>
        /// <param name="notification">Данные нового уведомления.</param>
        /// <returns>Созданное уведомление.</returns>
        /// <response code="200">Возвращает созданное уведомление.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateNotification req)
        {
            var cat = req.Adapt<Notification>();
            await _notificationService.Create(cat);
            return Ok();
        }
        /// <summary>
        /// Обновление существующего уведомления.
        /// </summary>
        /// <param name="notification">Данные для обновления уведомления.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если уведомление успешно обновлено.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateNotification req)
        {
            var cat = req.Adapt<Notification>();
            await _notificationService.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление уведомления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор уведомления.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если уведомление успешно удалено.</response>
        /// <response code="400">Если уведомление не найдено.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _notificationService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _notificationService.Delete(id);
            return Ok();
        }
    }
}