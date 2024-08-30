using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherForecastingApp.Application.Common.Exceptions;
using WeatherForecastingApp.Application.Common.Interfaces;
using WeatherForecastingApp.Domain.Entities;

namespace WeatherForecastingApp.Infrastructure.Services;
public class WeatherForeCastService : IWeatherForecastService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _weatherApiOptions;

    public WeatherForeCastService(HttpClient httpClient, IOptions<WeatherApiOptions>  weatherApiOptions)
    {
        _httpClient = httpClient;
        _weatherApiOptions = weatherApiOptions.Value;
    }

    public async Task<WeatherResponse> GetWeatherForecasts(string date, string city)
    {

        try
        {

            string apiUrl = $"{_weatherApiOptions.BaseUrl}/{city}/{date}?unitGroup=metric&key={_weatherApiOptions.ApiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);

                if (weatherData == null)
                {
                    throw new WeatherServiceException("Deserialized weather data is null. The JSON response might be malformed or does not match the model.");
                }
                if (weatherData!.Days!.Count > 0) { weatherData.Day = weatherData.Days[0]; }
                return weatherData;
            }
            else
            {

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new WeatherServiceException($"The requested resource or endpoint does not exist or cannot be found. Details: {responseBody}", (int)response.StatusCode);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new WeatherServiceException($"Check the city name '{city}' or date '{date}'. Details: {responseBody}", (int)response.StatusCode);
                }
                else
                {
                    throw new WeatherServiceException($"Error fetching weather data: {response.ReasonPhrase}. Details: {responseBody}", (int)response.StatusCode);
                }
            }
        }
        catch (HttpRequestException ex)
        {
            throw new WeatherServiceException("Error communicating with weather service", ex);
        }
        catch (Exception ex)
        {
            throw new WeatherServiceException("An unexpected error occurred in the weather service", ex);
        }
    }
}
