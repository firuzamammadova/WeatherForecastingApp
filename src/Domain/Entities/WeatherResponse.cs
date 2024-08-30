using System.Text.Json.Serialization;

namespace WeatherForecastingApp.Domain.Entities;

public class WeatherResponse
{
    [JsonPropertyName("resolvedAddress")]
    public string? Location { get; set; }

    [JsonPropertyName("days")]
    public List<WeatherDay>? Days { get; set; }

    [JsonPropertyName("day")]
    public WeatherDay? Day { get; set; }
}

public class WeatherDay
{
    [JsonPropertyName("datetime")]
    public DateTime Date { get; set; }

    [JsonPropertyName("tempmax")]
    public double MaxTemperature { get; set; }

    [JsonPropertyName("tempmin")]
    public double MinTemperature { get; set; }

    [JsonPropertyName("temp")]
    public double AverageTemperature { get; set; }

    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }

    [JsonPropertyName("precip")]
    public double Precipitation { get; set; }

    [JsonPropertyName("windspeed")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("conditions")]
    public string? Conditions { get; set; }
}
