using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace webApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
            "Freezing","Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{index}")]
        public string Getindex(int index)
        {
            var name = Summaries[index];
            return name;
        }

        [HttpGet("find-by-name")]
        public int Getfind_by_name(string name)
        {
            var count = Summaries.Count(c => c.Equals(name));
            return count;
        }

   

        [HttpPost]
        public IActionResult Add(string name)
         {
             if (Regex.IsMatch(name, @"^[а-яА-ЯёЁa-zA-Z]+$"))
             {
                 Summaries.Add(name);
             }
                 return Ok();    
         }

        [HttpPut]
        public IActionResult Update(int index, string name)
        {
            if (index<0 || index>=Summaries.Count)
            {
                return BadRequest("Èíäåêñ íå ìîæåò áûòü îòðèöàòåëüíûì èëè áîëüøå êîëè÷åñòâà ýëåìåíòîâ êîëè÷åñòâî ýëåìåíòîâ " + Summaries.Count);
            }
            Summaries[index] = name;
            return Ok();
        }


        [HttpDelete]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Èíäåêñ íå ìîæåò áûòü îòðèöàòåëüíûì èëè áîëüøå êîëè÷åñòâà ýëåìåíòîâ êîëè÷åñòâî ýëåìåíòîâ " + Summaries.Count);
            }
            Summaries.RemoveAt(index);
            return Ok();
        }




    }
}
