using WeatherForecast.Models;

namespace WeatherForecast.Services;

public interface IForecastService
{
    public Task UpdateOrCreateForecast(Forecast newForecast);
    public Task<City?> GetCityAsync(string name);
    public Task CreateCityAsync(
        string name,
        double latitude,
        double longtitude, 
        bool updateIfExists = false);
    public Task<Forecast?> GetForecastAsync(City city, DateOnly date);
    public Task DeleteForecastAsync(Forecast forecast);
    public Task DeleteCityAsync(City city);
}