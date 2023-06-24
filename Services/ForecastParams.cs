using System.Text;
namespace WeatherForecast.Services;

public class ForecastQueryParams
{
    public bool temperature = false;
    public bool windSpeed = false;
    public bool rain = false;
    public bool snowFall = false;
    public bool cloudCover = false;

    public ForecastQueryParams() 
    {
    }
    
    public override string ToString() 
    {
        var sb = new StringBuilder();
        sb.Append("&hourly=");
        if (temperature)
            sb.Append("temperature_2m,");
        if (windSpeed)
            sb.Append("windspeed_10m,");
        if (rain)
            sb.Append("rain,");
        if(snowFall)
            sb.Append("windspeed_10m,");
        if (cloudCover)
            sb.Append("cloudcover,");
        
        sb.Remove(sb.Length - 1, 1);
        var result = sb.ToString();
        
        if (result == "hourly")
            result = String.Empty;

        return result;
    } 
}