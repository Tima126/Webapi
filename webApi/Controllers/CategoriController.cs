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
    public class CategoriController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Получение всех категорий.
        /// </summary>
        /// <returns>Список всех категорий.</returns>
        /// <response code="200">Возвращает список категорий.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categor = await _categoryService.GetAll();
            return Ok(categor);
        }

        /// <summary>
        /// Получение категории по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <returns>Категория с указанным идентификатором.</returns>
        /// <response code="200">Возвращает категорию.</response>
        /// <response code="400">Если категория не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _categoryService.GetById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        /// <summary>
        /// Добавление новой категории.
        /// </summary>
        /// <param name="category">Данные новой категории.</param>
        /// <returns>Созданная категория.</returns>
        /// <response code="200">Возвращает созданную категорию.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategori req)
        {
            var cat = req.Adapt<Category>();
            await _categoryService.Create(cat);
            return Ok();
        }
        /// <summary>
        /// Обновление существующей категории.
        /// </summary>
        /// <param name="category">Данные для обновления категории.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="200">Если категория успешно обновлена.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateCategori req)
        {
            var cat = req.Adapt<Category>();
            await _categoryService.Update(cat);
            return NoContent();
        }

        /// <summary>
        /// Удаление категории по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если категория успешно удалена.</response>
        /// <response code="400">Если категория не найдена.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _categoryService.GetById(id);

            if (cat == null)
            {
                return BadRequest();
            }
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}