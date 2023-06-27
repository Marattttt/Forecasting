using WeatherForecast.Models;

namespace tests.Utils;

public static class DataGenerator
{
    public static Forecast GetRandomForecast(City city, DateOnly date)
    {
        return new Forecast() {
            City = city,
            ForecastDate = date,
            Temperature = randomForecastData(),
            WindSpeed = randomForecastData(),
            Rain = randomForecastData(),
            Snowfall = randomForecastData(),
            CloudCover = randomForecastData()
        };
    }

    // Returns an array of doubles between -20 and 20
    private static double[] randomForecastData() 
    {
        var rand = new Random();
        double[] res = Enumerable.Range(1, 24)
            .Select(_ => rand.NextDouble() * rand.Next(-20, 20))
            .ToArray();
        return res;
    }
}