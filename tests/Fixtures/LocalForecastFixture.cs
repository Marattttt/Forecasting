using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using WeatherForecast.Data;
using WeatherForecast.Models;

namespace tests.Fixtures;

public class LocalForecastFixture
{
    private readonly DbContextOptions<WeatherForecastContext> _dbOptions;
    private static readonly object _lock = new();
    private static bool _isDbInitialized = false;

    public LocalForecastFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var _connectionString = config.GetConnectionString("WeatherForecastTest")!;

        _dbOptions = new DbContextOptionsBuilder<WeatherForecastContext>()
            .UseNpgsql(_connectionString)
            .Options;
        _dbInit();
        
    }

    public WeatherForecastContext CreateContext()
        => new WeatherForecastContext(
            _dbOptions);

    private void _dbInit()
    {
        lock (_lock)
        {
            if (_isDbInitialized)
            {
                return;
            }

            City peru = new City("Peru", -12.04318, -77.02824);
            City bishkek = new City("Bishkek", 42.882, 74.582);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            using (var context = new WeatherForecastContext(_dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.AddRange(
                    new Forecast() {
                        City = peru,
                        ForecastDate = today,
                        Temperature = randomForecastData(),
                        WindSpeed = randomForecastData(),
                        Rain = randomForecastData(),
                        Snowfall = randomForecastData(),
                        CloudCover = randomForecastData()
                    },
                    new Forecast() {
                        City = bishkek,
                        ForecastDate = today,
                        Temperature = randomForecastData(),
                        WindSpeed = randomForecastData(),
                        Rain = randomForecastData(),
                        Snowfall = randomForecastData(),
                        CloudCover = randomForecastData()
                    }
                );
                
                context.SaveChanges();
            }
            _isDbInitialized = true;
        }
    }

    // Returns an array of doubles between -20 and 20
    private double[] randomForecastData() 
    {
        var rand = new Random();
        double[] res = Enumerable.Range(1, 24)
            .Select(_ => rand.NextDouble() * rand.Next(-20, 20))
            .ToArray();
        return res;
    }
}