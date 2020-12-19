using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.ASPNetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry.ASPNetCore.Services;

namespace OpenTelemetry.ASPNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherForecastApiService _weatherForecastApiService;

        public HomeController(ILogger<HomeController> logger, IWeatherForecastApiService weatherForecastApiService)
        {
            _logger = logger;
            _weatherForecastApiService = weatherForecastApiService;
        }

        public async Task<IActionResult> Index()
        {

            var weatherForecast = await _weatherForecastApiService.GetWeatherForecastAsync();
            
            return View(weatherForecast);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
