using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenTelemetry.ASPNetCore.ViewModels;

namespace OpenTelemetry.ASPNetCore.Services
{
    public class WeatherForecastApiService : IWeatherForecastApiService
    {
        private readonly HttpClient _client;

        public WeatherForecastApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<WeatherForecastViewModel>> GetWeatherForecastAsync()
        {
            var response = await _client.GetAsync("api/WeatherForecast");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<IList<WeatherForecastViewModel>>();
        }
    }
}
