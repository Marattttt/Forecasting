using Microsoft.Extensions.Configuration;

using WeatherForecast.Services;
using WeatherForecast.Models;

namespace tests;

public class OpenMeteoCommunicationTests
{
    readonly City ExampleCity = new City(
        "Bishkek",
        42.882,
        74.582);

    IConfigurationRoot config;
    public OpenMeteoCommunicationTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        config = builder.Build();
    }

    [Fact]
    public async Task Gives_Full_Forecast_For_Today()
    {

        var queryParams = new ForecastQueryParams(true);

        var service = new OpenMeteoCommunicationService(config);
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        
        List<Forecast> forecasts = await service.GetForecastsAsync(
            ExampleCity,
            currentDate,
            0,
            queryParams
        );
        Console.WriteLine(forecasts[0].Snowfall![0].ToString());
        Assert.Collection(forecasts,
            item => {
                Assert.NotNull(item.CloudCover);
                Assert.NotNull(item.Rain);
                Assert.NotNull(item.Snowfall);
                Assert.NotNull(item.Temperature);
                Assert.NotNull(item.WindSpeed);
            }
        );

        Assert.True(forecasts.Count() == 1, 
            "Method should return only one forecast for a single date with additional days set to 0; " + 
            $"Returned {forecasts.Count()} forecasts");
        
        Assert.Collection(forecasts, 
            item => {
                Assert.True(item.CloudCover!.Count() == 24);
                Assert.True(item.Rain!.Count() == 24);
                Assert.True(item.Snowfall!.Count() == 24);
                Assert.True(item.Temperature!.Count() == 24);
                Assert.True(item.WindSpeed!.Count() == 24);
            });
    }

    [Fact]
    public async Task Throws_On_No_Query_Params_Provided()
    {
        var queryParams = new ForecastQueryParams();

        var service = new OpenMeteoCommunicationService(config);

        var result = new List<Forecast>();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => result = await service.GetForecastsAsync(
                ExampleCity,
                DateOnly.FromDateTime(DateTime.Now),
                0,
                queryParams
            )
        );

    }
}