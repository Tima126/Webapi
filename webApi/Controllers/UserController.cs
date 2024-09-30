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
    public class UserController : ControllerBase
    {

        private readonly IUserService _user;

        public UserController(IUserService user)
        {
            _user = user;
        }




        /// <summary>
        /// Получение всех продуктов.
        /// </summary>
        /// <returns>Список всех продуктов.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _user.GetAll();
            return Ok(products);
        }

        /// <summary>
        /// Получение пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>пользователь с указанным идентификатором.</returns>
        /// <response code="200">Возвращает пользователя.</response>
        /// <response code="404">Если пользователь не найден.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _user.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="product">Данные нового пользователя.</param>
        /// <returns>Созданние пользователя.</returns>
        /// <response code="201">Возвращает созданного пользователя.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUser req)
        {
            var user = req.Adapt<User>();
            await _user.Create(user);
            return Ok();
        }


        /// <summary>
        /// Обновление существующего продукта.
        /// </summary>
        /// <param name="product">Данные для обновления пользователя.</param>
        /// <returns>Результат обновления.</returns>
        /// <response code="204">Если пользователь успешно обновлен.</response>
        [HttpPut]
        public async Task<IActionResult> Update(CreateProducts req)
        {
            var user = req.Adapt<User>();
            await _user.Update(user);

            return NoContent();
        }
        /// <summary>
        /// Удаление пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Результат удаления.</returns>
        /// <response code="200">Если пользователь успешно удален.</response>
        /// <response code="400">Если пользователь не найден.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _user.GetById(id);

            if (user == null)
            {
                return BadRequest();
            }
            await _user.Delete(id);
            return Ok();
        }


    }
}
