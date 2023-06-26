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
    
    public static readonly City city1 = new City("Peru", -12.043, -77.028);
    public static readonly City city2 = new City("Bishkek", 42.882, 74.582);
    public static readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

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

    public WeatherForecastContext CreateContext() =>
        new WeatherForecastContext(_dbOptions);

    private void _dbInit()
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
                    new Forecast() {
                        City = city1,
                        ForecastDate = today,
                        Temperature = randomForecastData(),
                        WindSpeed = randomForecastData(),
                        Rain = randomForecastData(),
                        Snowfall = randomForecastData(),
                        CloudCover = randomForecastData()
                    },
                    new Forecast() {
                        City = city2,
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