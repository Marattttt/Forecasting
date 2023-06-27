using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data;
using WeatherForecast.Models;

namespace WeatherForecast.Services;

public class LocalForecastRepository : IForecastRepository
{
    WeatherForecastContext _context;

    public LocalForecastRepository(WeatherForecastContext context) 
    {
        _context = context;
    }

    public async Task<City?> GetCityAsync(string name) 
    {
        City? city = await _context.Cities
            .OrderBy(c => c.Name)
            .Where(c => c.Name == name)
            .FirstOrDefaultAsync();
        return city;
    }
    public async Task<Forecast?> GetForecastAsync(City city, DateOnly date)
    {
        Forecast? forecast = await _context.Forecasts
            .FindAsync(city.CityId, date);
        return forecast;
    }

    public async Task CreateCityAsync(
        string name,
        double latitude,
        double longtitude, 
        bool updateIfExists = false)
    {
        City newCity = new City();
        newCity.Location = new NpgsqlTypes.NpgsqlPoint(latitude, longtitude);
        newCity.Name = name;
    
        City? old = await GetCityAsync(name);
        if (old is null) 
        {
            await _context.Cities.AddAsync(newCity);
            await _context.SaveChangesAsync();
            return;
        }

        if (updateIfExists)
        {
            old.Location = newCity.Location;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateOrCreateForecast(Forecast newForecast)
    {
        var existingForecast = await GetForecastAsync(
            newForecast.City, newForecast.ForecastDate);

        if (existingForecast != null)
        {
            existingForecast.CloudCover = newForecast.CloudCover;
            existingForecast.Rain = newForecast.Rain;
            existingForecast.Snowfall = newForecast.Snowfall;
            existingForecast.Temperature = newForecast.Temperature;
            existingForecast.WindSpeed = newForecast.WindSpeed;
            _context.Entry(existingForecast).State = EntityState.Modified;
        }
        else
        {
            _context.Entry(newForecast.City).State = EntityState.Detached;
            _context.Entry(newForecast).State = EntityState.Added;
            await _context.Forecasts.AddAsync(newForecast);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCityAsync(City city) 
    {
        _context.Remove(city);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteForecastAsync(Forecast forecast) 
    {
        _context.Remove(forecast);
        await _context.SaveChangesAsync();
    }
}