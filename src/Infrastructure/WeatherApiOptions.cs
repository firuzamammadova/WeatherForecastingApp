using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastingApp.Infrastructure;
public class WeatherApiOptions
{
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
}
