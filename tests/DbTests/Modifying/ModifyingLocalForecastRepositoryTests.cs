using WeatherForecast.Models;
using WeatherForecast.Services;

using tests.Utils;
using tests.Fixtures;

namespace tests;

public class ModifyingLocalForecastRepositoryTests : IClassFixture<LocalForecastFixture>
{
    private LocalForecastFixture _fixture { get; }

    public ModifyingLocalForecastRepositoryTests(LocalForecastFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("City", 32.3, 41.942)]
    [InlineData("Город", -1.2, 0)]
    public async Task Adds_New_City(string name, double latitude, double lontitude)
    {
        var newCity = new City(name, latitude, lontitude);
        using (var context = _fixture.CreateContext())
        {
            context.Database.BeginTransaction();

            var repository = new LocalForecastRepository(context);
            await repository.CreateCityAsync(name, latitude, lontitude);

            City? addedCity = context.Cities.First(c => c.Name == newCity.Name);

            Assert.NotNull(addedCity);
            
            Assert.Equal(newCity, addedCity);
        }
    }

    [Fact]
    public async void Adds_New_Forecast()
    {
        var forecast = DataGenerator.GetRandomForecast(
            _fixture.city1,
            _fixture.today.AddDays(1)
        );

        using (var context = _fixture.CreateContext())
        {   
            context.Database.BeginTransaction();

            var repository = new LocalForecastRepository(context);

            await repository.UpdateOrCreateForecast(forecast);

            Forecast? addedForecast = context.Forecasts.First(
                f => f.City == forecast.City && f.ForecastDate == forecast.ForecastDate
            );

            Assert.NotNull(addedForecast);

            Assert.Equal(forecast, addedForecast);
        }
    }

    [Fact]
    public async void Updates_Forecast()
    {
        var newForecast = DataGenerator.GetRandomForecast(
            _fixture.city1,
            _fixture.today
        );

        using (var context = _fixture.CreateContext())
        {   
            context.Database.BeginTransaction();

            var repository = new LocalForecastRepository(context);

            var oldTemperature = new double[24];
            var oldForecast = await repository.GetForecastAsync(newForecast.City, _fixture.today);
            oldForecast!.Temperature!.CopyTo(oldTemperature, 0);

            await repository.UpdateOrCreateForecast(newForecast);

            var updatedForecast = await repository.GetForecastAsync(newForecast.City, _fixture.today);

            Assert.Contains(
                context.Forecasts, 
                f => f.ForecastDate == newForecast.ForecastDate);

            Assert.NotSame(oldTemperature, updatedForecast!.Temperature);
        }
    }

    [Fact]
    public async Task Deletes_Forecast()
    {
        using (var context = _fixture.CreateContext())
        {
            context.Database.BeginTransaction();
            var repository = new LocalForecastRepository(context);

            var forecast = context.Forecasts.First(f => f.City == _fixture.city1);
            await repository.DeleteForecastAsync(forecast);

            Assert.DoesNotContain(
                context.Forecasts, 
                f => f.City == _fixture.city1 && f.ForecastDate == _fixture.today);
        }
    }
}