using AutoMapper;
using WeatherForecastingApp.Application.Common.Exceptions;
using WeatherForecastingApp.Application.Common.Interfaces;

namespace WeatherForecastingApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;

public record GetWeatherForecastsQuery : IRequest<WeatherForecast>
{
    public string? Date { get; init; }
    public string? City { get; init; }
}
public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, WeatherForecast>
{
    private readonly IWeatherForecastService _service;
    private readonly IMapper _mapper;

    public GetWeatherForecastsQueryHandler(IWeatherForecastService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<WeatherForecast> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
    {

        var response = await _service.GetWeatherForecasts(request.Date!, request.City!);

        WeatherForecast weatherForecast = _mapper.Map<WeatherForecast>(response);
        return weatherForecast;


    }
}
