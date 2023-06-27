using System;
using Microsoft.AspNetCore.Mvc;

using WeatherForecast.Models;
using WeatherForecast.Services;
namespace WeatherForecast.Controllers;

[ApiController]
[Route("cities")]
public class CitiesContoller : ControllerBase
{
    IForecastRepository _forecastRepository;
    public CitiesContoller(
        LocalForecastRepository forecastRepository)
    {
        _forecastRepository = forecastRepository;
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
        City? old = await _forecastRepository.GetCityAsync(name);
        if (old is not null) 
        {
            return BadRequest("City is present in database");
        }

        await _forecastRepository.CreateCityAsync(name, latitude, longtitude, updateIfExists);

        return Ok();
    }
}
