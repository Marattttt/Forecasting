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

            // forecast.City = (await service.GetCityAsync(forecast.City.Name))!;

            await repository.UpdateOrCreateForecast(forecast);

            Assert.Contains(
                context.Forecasts, 
                f => f.ForecastDate == forecast.ForecastDate);

            // context.ChangeTracker.Clear();
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

            // newForecast.City = context.Cities.First(c => c.Name == newForecast.City.Name);
            // newForecast.CityId = newForecast.City.CityId;

            var oldTemperature = new double[24];
            var oldForecast = await repository.GetForecastAsync(newForecast.City, _fixture.today);
            oldForecast!.Temperature!.CopyTo(oldTemperature, 0);

            await repository.UpdateOrCreateForecast(newForecast);

            var updatedForecast = await repository.GetForecastAsync(newForecast.City, _fixture.today);

            Assert.Contains(
                context.Forecasts, 
                f => f.ForecastDate == newForecast.ForecastDate);

            Assert.NotSame(oldTemperature, updatedForecast!.Temperature);

            // context.ChangeTracker.Clear();
        }
    }
}