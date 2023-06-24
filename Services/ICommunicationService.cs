using WeatherForecast.Models;

namespace WeatherForecast.Services;

public interface ICommnunicationService
{
    public Task<List<Forecast>> GetForecastsAsync(
        City city, 
        DateOnly start,
        int days,
        ForecastQueryParams queryParams);
}