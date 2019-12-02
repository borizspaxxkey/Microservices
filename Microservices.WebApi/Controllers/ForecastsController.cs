using Microservices.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Microservices.WebApi.Controllers
{
    public class WeatherForecast
    {
        [Required]
        public string DateFormatted { get; set; }
        [Required]
        public int TemperatureC { get; set; }
        [Required]
        public string Summary { get; set; }
    }

    [Route("api/[controller]")]
    public class ForecastsController : Controller
    {
        private IForecastService _service;

        public ForecastsController(IForecastService service)
        {
            _service = service;
        }

        // GET api/forecasts
        [HttpGet]
        [Authorize("read:forecasts")] 
        public IActionResult GetAll()
        {
            try
            {
                var result = _service.QueryAll();
                return Ok(result);
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
            }

        }

        //POST api/forecasts
        [HttpPost]
        [Authorize("write:forecasts")]
        public IActionResult SaveForecast([FromBody]WeatherForecast forecast)
        {
            try
            {
                var valid = TryValidateModel(forecast);
                return Ok(forecast);
            }
            catch
            {
                return BadRequest();

            }
        }

    }
}
