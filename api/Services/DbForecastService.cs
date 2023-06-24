using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data;
using WeatherForecast.Models;

namespace WeatherForecast.Services;

public class DbForecastService : IForecastService
{
    WeatherForecastContext _context;

    public DbForecastService(WeatherForecastContext context) 
    {
        _context = context;
    }

    public async Task<City?> GetCityAsync(string name) 
    {
        City? city = await _context.Cities
            .Where(c => c.Name == name)
            .OrderBy(c => c.Name)
            .FirstOrDefaultAsync();
        return city;
    }
    public async Task<Forecast?> GetForecastAsync(City city, DateOnly date)
    {
        Forecast? forecast = await _context.Forecasts
            .Where(f => f.CityId == city.CityId && f.ForecastDate == date)
            .FirstOrDefaultAsync();
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
        City? city = await GetCityAsync(newForecast.City.Name);

        if (city is null)
        {
            throw new ArgumentNullException("City not found");
        }

        newForecast.CityId = city.CityId;
        var existingForecast = await GetForecastAsync(
            city, newForecast.ForecastDate);

        if (existingForecast != null)
        {
            Console.WriteLine("Entity already exists");
            existingForecast.CloudCover = newForecast.CloudCover;
            existingForecast.Rain = newForecast.Rain;
            existingForecast.Snowfall = newForecast.Snowfall;
            existingForecast.Temperature = newForecast.Temperature;
            existingForecast.WindSpeed = newForecast.WindSpeed;
            _context.Entry(existingForecast).State = EntityState.Modified;
        }
        else
        {
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