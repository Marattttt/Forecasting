using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using tests.Fixtures;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace tests;

public class DbForecastServiceTests : IClassFixture<LocalForecastFixture>
{
    // private readonly TestServer _server;
    // private readonly HttpClient _client;

    public LocalForecastFixture Fixture { get; }

    public DbForecastServiceTests(LocalForecastFixture fixture)
    {
        // _server = new TestServer(new WebHostBuilder()
        //    .UseStartup<Program>());
        // _client = _server.CreateClient();

        Fixture = fixture;
    }

    [Fact]
    public async Task ReturnsOnlyExistingCities()
    {
        City? city1 = null;
        City? city2 = null;
        City? fictionalCity = null;

        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            city1 = await service.GetCityAsync("Bishkek");
            city2 = await service.GetCityAsync("Peru");
            fictionalCity = await service.GetCityAsync("abcdef");
        }
        Assert.NotNull(city1);
        Assert.NotNull(city2);
        Assert.Null(fictionalCity);
    }
}