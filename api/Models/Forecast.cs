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

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return this is null;
        
        var f = obj as Forecast;
        if (f is null)
            return false;
        
        if (Temperature is not null && f.Temperature is not null)
        {
            if (!Enumerable.SequenceEqual(Temperature, f.Temperature))
                return false;
        } 
        else if (Temperature is null || f.Temperature is null)
        {
            return false;
        }
        if (WindSpeed is not null && f.WindSpeed is not null)
        {
            if (!Enumerable.SequenceEqual(WindSpeed, f.WindSpeed))
                return false;
        } 
        else if (WindSpeed is null || f.WindSpeed is null)
        {
            return false;
        }
        if (Rain is not null && f.Rain is not null)
        {
            if (!Enumerable.SequenceEqual(Rain, f.Rain))
                return false;
        } 
        else if (Rain is null || f.Rain is null)
        {
            return false;
        }
        if (Snowfall is not null && f.Snowfall is not null)
        {
            if (!Enumerable.SequenceEqual(Snowfall, f.Snowfall))
                return false;
        } 
        else if (Snowfall is null || f.Snowfall is null)
        {
            return false;
        }
        if (CloudCover is not null && f.CloudCover is not null)
        {
            if (!Enumerable.SequenceEqual(CloudCover, f.CloudCover))
                return false;
        } 
        else if (CloudCover is null || f.CloudCover is null)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash *= 486187739 * City.GetHashCode();
            hash *= 486187739 * ForecastDate.GetHashCode();
            hash *= 486187739 * Snowfall?.GetHashCode() ?? 53;
            hash *= 486187739 * CloudCover?.GetHashCode() ?? 7;
            hash *= 486187739 * WindSpeed?.GetHashCode() ?? 997;
            hash *= 486187739 * Rain?.GetHashCode() ?? 13;
            hash *= 486187739 * Temperature?.GetHashCode() ?? 103;
            hash *= 486187739 * Snowfall?.GetHashCode() ?? 347;
            return hash;
        }
    }

    
}
