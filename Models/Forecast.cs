using System.Text.Json.Serialization;

namespace WeatherForecast.Models;

public class Forecast
{
    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public DateOnly ForecastDate { get; set; }
    
    [JsonPropertyName("temperature_2m")]   
    public double[]? Temperature { get; set; } 

    [JsonPropertyName("windspeed_10m")]
    public double[]? WindSpeed { get; set; } 

    [JsonPropertyName("rain")]
    public double[]? Rain { get; set; } 

    [JsonPropertyName("snowfall")]
    public double[]? Snowfall { get; set; } 

    [JsonPropertyName("cloudcover")]
    public double[]? CloudCover { get; set; }
    
}
