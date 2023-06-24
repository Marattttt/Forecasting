using NpgsqlTypes;
namespace WeatherForecast.Models;

public class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;
    public NpgsqlPoint Location { get; set; }

    public virtual ICollection<Forecast> Forecasts { get; } = new List<Forecast>();
}
