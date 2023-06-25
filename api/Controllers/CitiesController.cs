using System;
using Microsoft.AspNetCore.Mvc;

using WeatherForecast.Models;
using WeatherForecast.Services;
namespace WeatherForecast.Controllers;

[ApiController]
[Route("cities")]
public class CitiesContoller : ControllerBase
{
    IForecastService _forecastService;
    public CitiesContoller(
        LocalForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    // /cities/new
    [HttpPost()]
    [Route("new")]
    public async Task<ActionResult> NewCity(
        [FromForm] string name,
        [FromForm] double latitude,
        [FromForm] double longtitude,
        [FromForm] bool updateIfExists = false)
    {
        City? old = await _forecastService.GetCityAsync(name);
        if (old is not null) 
        {
            return BadRequest("City is present in database");
        }

        await _forecastService.CreateCityAsync(name, latitude, longtitude, updateIfExists);

        return Ok();
    }
}
