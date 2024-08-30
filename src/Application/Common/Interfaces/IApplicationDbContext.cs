using WeatherForecastingApp.Domain.Entities;

namespace WeatherForecastingApp.Application.Common.Interfaces;
public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
