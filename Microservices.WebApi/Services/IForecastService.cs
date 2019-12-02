using Microservices.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.WebApi.Services
{
   public interface IForecastService
    {
        IEnumerable<WeatherForecast> QueryAll();
    }

    public class ForecastService : IForecastService
    {
        public IEnumerable<WeatherForecast> QueryAll()
        {
            return new List<WeatherForecast>();
        }
    }
}
