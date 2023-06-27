using NpgsqlTypes;
namespace WeatherForecast.Models;

public class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;
    public NpgsqlPoint Location { get; set; }
    public ICollection<Forecast> Forecasts { get; } = new List<Forecast>();

    public City() 
    {
    }

    public City(string name, double latitude, double longtitude)
    {
        Name = name;
        Location = new NpgsqlPoint(latitude, longtitude);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return this is null;
        
        var c = obj as City;
        if (c is null)
            return false;
        
        if (Name != c.Name || Location != c.Location || Forecasts.Count() != c.Forecasts.Count())
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash *= 486187739 * Name.GetHashCode();
            hash *= 486187739 * Location.GetHashCode();
            hash *= 486187739 * Forecasts.GetHashCode();
            return hash;
        }
    }
}
