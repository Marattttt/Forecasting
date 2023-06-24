using System.Text.Json.Serialization;

using WeatherForecast.Models;

namespace WeatherForecast.Json;

public class Root
{
    [JsonPropertyName("hourly")]
    public Forecast Hourly { get; set; } = null!;
}