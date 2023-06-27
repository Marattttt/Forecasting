using System;
using Microsoft.AspNetCore.Mvc;

using WeatherForecast.Models;
using WeatherForecast.Services;
namespace WeatherForecast.Controllers;

[ApiController]
[Route("forecasts")]
public class ForecastsController : ControllerBase
{
    readonly TimeSpan maxSupportedHistory = new TimeSpan(200,0,0,0); 
    readonly TimeSpan maxSupportedFuture = new TimeSpan(-15,0,0,0);
    ICommnunicationService _communicationService;
    IForecastRepository _forecastRepository;
    public ForecastsController(
        IConfiguration configuration, 
        LocalForecastRepository forecastRepository, 
        OpenMeteoCommunicationService communicationService)
    {
        _forecastRepository = forecastRepository;
        _communicationService = communicationService;
    }

    // /forecasts/new/
    [HttpPost()]
    [Route("new")]
    public async Task<ActionResult> LoadForecasts(
        [FromForm] string cityName, 
        [FromForm] int year,
        [FromForm] int month,
        [FromForm] int day,
        [FromForm] int additionalDays = 0,
        [FromForm] bool includeTemperature = false,
        [FromForm] bool includeWindSpeed = false,
        [FromForm] bool includeRain = false,
        [FromForm] bool includeSnowFall = false,
        [FromForm] bool includeCloudCover = false)
    {
        City? city = await _forecastRepository.GetCityAsync(cityName);
        var queryParams = new ForecastQueryParams() {
            temperature = includeTemperature,
            windSpeed = includeWindSpeed,
            rain = includeRain,
            snowFall = includeSnowFall,
            cloudCover = includeCloudCover
        };

        if (city is null) 
        {
            return BadRequest("City not found in database");
        }

        if (queryParams.ToString() == String.Empty) 
        {
            return BadRequest("No query parameters provided");
        }

        DateOnly start;
        try {
            start = new DateOnly(year,month,day);
        }  
        catch(ArgumentOutOfRangeException)
        {
            return BadRequest("Wrong data format");
        }
        
        TimeSpan differenceMax = DateTime.Now.Subtract(
            start.AddDays(additionalDays)
            .ToDateTime(new TimeOnly()));
        TimeSpan differenceMin = DateTime.Now.Subtract(
            start
            .ToDateTime(new TimeOnly()));

        if(differenceMin > maxSupportedHistory || differenceMax < maxSupportedFuture)
        {
            return BadRequest("Only 200 days of history and 15 days of future are supported");
        }

        List<Forecast> forecasts;
        try {
            forecasts = await _communicationService.GetForecastsAsync(city, start, additionalDays, queryParams);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        foreach (Forecast forecast in forecasts) 
        {
            await _forecastRepository.UpdateOrCreateForecast(forecast);
        }

        return Ok();
    }
}
