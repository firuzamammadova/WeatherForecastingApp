using WeatherForecastingApp.Domain.Entities;

namespace WeatherForecastingApp.Application.Common.Interfaces;
public interface IWeatherForecastService
{
    Task<WeatherResponse> GetWeatherForecasts(string date, string city);

}
