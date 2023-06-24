using System.Text;
using System.Text.Json;

using WeatherForecast.Models;
using WeatherForecast.Json;

namespace WeatherForecast.Services;

public class OpenMeteoCommunicationService : ICommnunicationService
{
    IConfiguration _config;
    public OpenMeteoCommunicationService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task<List<Forecast>> GetForecastsAsync(
        City city, 
        DateOnly start,
        int days,
        ForecastQueryParams queryParams)
    {
        if (queryParams.ToString() == String.Empty) 
            throw new ArgumentException("No query params given");
        
        string baseUrl = (string)_config.GetValue(typeof(string), "OpenMeteoUrlRoot");
        var sb = new StringBuilder();
        sb.Append(baseUrl);
        sb.Append("forecast?");
        sb.Append("longitude=" + city.Location.X);
        sb.Append("&latitude=" + city.Location.Y);
        sb.Append(queryParams.ToString());
        sb.Append("&timezone=auto");

        List<Forecast> result = new List<Forecast>();

        DateOnly end = start.AddDays(days);

        for ( ; start <= end; start = start.AddDays(1) )
        {   
            Forecast forecast;
            try 
            {
                forecast = await GetForecast(sb.ToString(), start, city);
            } 
            catch (NullReferenceException e)
            {
                throw e;
            }

            result.Add(forecast);
        }

        return result;
        
    }
    private async Task<Forecast> GetForecast(string baseUrl, DateOnly date, City city)
    {
        string finalUrl = baseUrl + "&start_date=" + date.ToString("o") + "&end_date=" + date.ToString("o");
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(finalUrl);

            Console.WriteLine(finalUrl);
            
            JsonDocument jsonDocument = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            JsonElement rootElement = jsonDocument.RootElement;

            var root = rootElement.Deserialize<Root>();
            var data = new Forecast();
            Console.WriteLine(
                await response.Content.ReadAsStringAsync()
            );
            Console.WriteLine("\n\n\n");

            try 
            {
                data = root!.Hourly;
            }
            catch (NullReferenceException e)
            {
                throw e;
            }

            data.City = city;
            data.ForecastDate = date;
            return data;
        }
    }
}