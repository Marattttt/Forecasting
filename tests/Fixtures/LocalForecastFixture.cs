using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using WeatherForecast.Data;
using WeatherForecast.Models;

using tests.Utils;

namespace tests.Fixtures;

public class LocalForecastFixture
{
    private readonly DbContextOptions<WeatherForecastContext> _dbOptions;
    private static readonly object _lock = new();
    private static bool _isDbInitialized = false;

    public readonly City city1 = new City("Peru", -12.043, -77.028);
    public readonly City city2 = new City("Bishkek", 42.882, 74.582);
    public readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

    public LocalForecastFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var _connectionString = config.GetConnectionString("WeatherForecastTest")!;

        _dbOptions = new DbContextOptionsBuilder<WeatherForecastContext>()
            .UseNpgsql(_connectionString)
            .EnableSensitiveDataLogging()
            .Options;
            
        dbInit();
    }

    public WeatherForecastContext CreateContext()
    {
        var context = new WeatherForecastContext(_dbOptions);
        city1.CityId = context.Cities.First(c => c.Name == city1.Name).CityId;
        city2.CityId = context.Cities.First(c => c.Name == city2.Name).CityId;
        return context;
    }

    private void dbInit()
    {
        lock (_lock)
        {
            if (_isDbInitialized)
            {
                return;
            }

            using (var context = new WeatherForecastContext(_dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.AddRange(
                    DataGenerator.GetRandomForecast(city1, today),
                    DataGenerator.GetRandomForecast(city2, today)
                );

                context.SaveChanges();
            }
            _isDbInitialized = true;
        }
    }
}