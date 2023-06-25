using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using tests.Fixtures;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace tests;

public class LocalForecastServiceTests : IClassFixture<LocalForecastFixture>
{
    // private readonly TestServer _server;
    // private readonly HttpClient _client;

    public LocalForecastFixture Fixture { get; }

    public LocalForecastServiceTests(LocalForecastFixture fixture)
    {
        // _server = new TestServer(new WebHostBuilder()
        //    .UseStartup<Program>());
        // _client = _server.CreateClient();

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
    public async Task Returns_Null_For_Not_Saved_City (string cityName)
    {
        City? city = null;

        using (var context = Fixture.CreateContext())
        {
            var service = new LocalForecastService(context);
            city = await service.GetCityAsync(cityName);
        }
        Assert.Null(city);
    }
}