using tests.Fixtures;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace tests;

public class ReadOnlyLocalForecastRepositoryTests : IClassFixture<LocalForecastFixture>
{
    private LocalForecastFixture _fixture { get; }

    public ReadOnlyLocalForecastRepositoryTests(LocalForecastFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("Bishkek")]
    [InlineData("Peru")]
    public async Task Returns_Existing_City(string cityName)
    {
        City? city = null;

        using (var context = _fixture.CreateContext())
        {
            var repository = new LocalForecastRepository(context);
            city = await repository.GetCityAsync(cityName);
        }
        Assert.NotNull(city);
    }

    [Theory]
    [InlineData("abcd")]
    public async Task Returns_Null_For_Absent_City (string cityName)
    {
        City? city = null;

        using (var context = _fixture.CreateContext())
        {
            var repository = new LocalForecastRepository(context);
            city = await repository.GetCityAsync(cityName);
        }
        Assert.Null(city);
    }

    [Fact]
    public async Task Returns_Existing_Forecast()
    {
        Forecast? forecast = null;
        using (var context = _fixture.CreateContext())
        {
            var repository = new LocalForecastRepository(context);
            forecast = await repository.GetForecastAsync(
                _fixture.city1,
                _fixture.today
            );
        }
        Assert.NotNull(forecast);
    }

    [Fact]
    public async Task Returns_Null_For_Absent_Forecast()
    {
        Forecast? forecast = null;
        using (var context = _fixture.CreateContext())
        {
            var repository = new LocalForecastRepository(context);
            forecast = await repository.GetForecastAsync(
                _fixture.city1,
                _fixture.today.AddDays(1)
            );
        }
        Assert.Null(forecast);
    }
}