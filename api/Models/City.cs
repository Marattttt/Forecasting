using NpgsqlTypes;
namespace WeatherForecast.Models;

public class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;
    public NpgsqlPoint Location { get; set; }
    public virtual ICollection<Forecast> Forecasts { get; } = new List<Forecast>();

    public City() 
    {
    }

    public City(string name, double latitude, double longtitude)
    {
        Name = name;
        Location = new NpgsqlPoint(latitude, longtitude);
    }
}
