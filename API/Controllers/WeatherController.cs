using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWeather()
        {
            var weather = new { Location = "New York", Temperature = 72 };
            return Ok(weather);
        }
    }
}
