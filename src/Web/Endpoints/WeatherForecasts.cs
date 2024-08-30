using WeatherForecastingApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WeatherForecastingApp.Web.Endpoints;
public class WeatherForecasts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetWeatherForecasts);
    }

    public  Task<WeatherForecast> GetWeatherForecasts(ISender sender, [AsParameters] GetWeatherForecastsQuery query)
    {
        return sender.Send(query);

    }


}
