using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace webApi.Controllers
{

    public class WeatherData
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Degree { get; set; }
        public string Location { get; set; }

    }





    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
            "Freezing","Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public static List<WeatherData> weatherDatas = new()
        {

            new WeatherData() {Id = 1, Date="21.01.2022",Degree=10, Location="Мурманск"},
            new WeatherData() {Id = 1, Date="21.01.2022",Degree=10, Location="Москва"},
            new WeatherData() {Id = 1, Date="21.01.2022",Degree=10, Location="Санкт-петербург"},
        };


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public List<WeatherData> GetAll()
        {
            return weatherDatas;
        }

        [HttpPost]
        public IActionResult Add(WeatherData data)
        {
            if (data.Id < 0)
            {
                return BadRequest("id не может быть меньше 0");
            }
            for (int i = 0; i < weatherDatas.Count; i++)
            {

                if (weatherDatas[i].Id == data.Id)
                {
                    return BadRequest("Запись таким id уже есть");
                }
            }
            weatherDatas.Add(data);
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult Getindex(int id)
        {
            if (id < 0)
            {
                return BadRequest("id не может быть отрицательным");
            }
            for (int i = 0; i < weatherDatas.Count; i++)
            {
                if (weatherDatas[i].Id == id)
                {
                    return Ok(weatherDatas[i]);
                }
            }

            return BadRequest("Такая запись не обнаружена");
        }

        [HttpGet("find-city-name")]

        public IActionResult GetSity(string local)
        {
            for (int i = 0; i < weatherDatas.Count; i++)
            {
                if (local == weatherDatas[i].Location)
                {
                    return BadRequest("Такой город найден, по id "+ i);
                }


            }

            return BadRequest("Не найден");

        }











    }
}
