
using System.Globalization;

namespace WeatherForecastingApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;
public class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(v => v.City)
            .NotEmpty();
        RuleFor(v => v.Date)
           .NotEmpty()
            .Must(BeAValidDate)
            .WithMessage("Date format is invalid. Format must be yyyy-M-d['T'H:m:s][.SSS][X].");

    }

    private bool BeAValidDate(string? date)
    {
        // Handle null values here if necessary
        if (string.IsNullOrEmpty(date))
        {
            return false;
        }

        // Parse the date with the expected format
        return DateTime.TryParseExact(date, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}
