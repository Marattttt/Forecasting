using System.Net.Security;
using tests.Fixtures;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace tests;

public class LocalForecastServiceReadOnlyTests : IClassFixture<LocalForecastFixture>
{
    public LocalForecastFixture Fixture { get; }

    public LocalForecastServiceReadOnlyTests(LocalForecastFixture fixture)
    {
        Fixture = fixture;
    }

    [Theory]
    [InlineData("Bishkek")]
    [InlineData("Peru")]
    public async Task Returns_Existing_City(string cityName)
    {
        City? city = null;

        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            city = await service.GetCityAsync(cityName);
        }
        Assert.NotNull(city);
    }

    [Theory]
    [InlineData("abcd")]
    public async Task Returns_Null_For_Absent_City (string cityName)
    {
        City? city = null;

        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            city = await service.GetCityAsync(cityName);
        }
        Assert.Null(city);
    }

    [Fact]
    public async Task Returns_Existing_Forecast()
    {
        Forecast? forecast = null;
        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            forecast = await service.GetForecastAsync(
                LocalForecastFixture.city1,
                LocalForecastFixture.today
            );
        }
        Assert.NotNull(forecast);
    }

    [Fact]
    public async Task Returns_Null_For_Absent_Forecast()
    {
        Forecast? forecast = null;
        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            forecast = await service.GetForecastAsync(
                LocalForecastFixture.city1,
                LocalForecastFixture.today.AddDays(1)
            );
        }
        Assert.Null(forecast);
    }
}