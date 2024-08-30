using System.Text.Json.Serialization;
using WeatherForecastingApp.Domain.Entities;

namespace WeatherForecastingApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;

public class WeatherForecast
{
    public DateTime Date { get; init; }
    public string? Location { get; set; }

    public double MaxTemperature { get; set; }

    public double MinTemperature { get; set; }

    public double AverageTemperature { get; set; }

    public double Humidity { get; set; }

    public double Precipitation { get; set; }

    public double WindSpeed { get; set; }

    public string? Conditions { get; set; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<WeatherResponse, WeatherForecast>()
                       .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Day != null ? src.Day.Date : default(DateTime)))
                       .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                       .ForMember(dest => dest.MaxTemperature, opt => opt.MapFrom(src => src.Day != null ? src.Day.MaxTemperature : 0))
                       .ForMember(dest => dest.MinTemperature, opt => opt.MapFrom(src => src.Day != null ? src.Day.MinTemperature : 0))
                       .ForMember(dest => dest.AverageTemperature, opt => opt.MapFrom(src => src.Day != null ? src.Day.AverageTemperature : 0))
                       .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Day != null ? src.Day.Humidity : 0))
                       .ForMember(dest => dest.Precipitation, opt => opt.MapFrom(src => src.Day != null ? src.Day.Precipitation : 0))
                       .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.Day != null ? src.Day.WindSpeed : 0))
                       .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.Day != null ? src.Day.Conditions : null));
        }
    }
}


