using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry.ASPNetCore.ViewModels;

namespace OpenTelemetry.ASPNetCore.Services
{
    public interface IWeatherForecastApiService
    {
        Task<IEnumerable<WeatherForecastViewModel>> GetWeatherForecastAsync();
    }
}
