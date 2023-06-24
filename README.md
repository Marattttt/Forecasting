# Forecasting.Net

An api for archiving and viewing meteo data from around the world

## Configuring

1. To change data about connections/user in DB, changes should be added to Init.sql and appsettings.json (change ConnectionStrings/WeatherForecast value to your new connection string)
2. By default, the application uses ports 7000 for https and 5000 for http requests (can be changed in appsettings.json)

## Deployment

    *Requires .net sdk and postgres version 14+*

1. Execute Init.sql as root in PostgreSQL

    - Creates database named weather_foreacast
    - Creates user named user_name with password "user_passowrd" and gives it read/write privileges on the weather_forecast database
    - Inserts into the "cities" table data about the city of Bishkek

2. Build and run the project from terminal by executing "dotnet run" in the project's root directory

## Details

1. Open-Meteo: loading history data of up to 200 days and future forecasts of up to 15 days is supported
2. Forecast data is stored in arrays of 24 numbers representing 24 hours of a day; One forecast = one day in one city

## Using

Applications base link will be printed out to terminal after the "dotnet run". To access swagger UI copy it to a browser and append with "/swagger/index.html". Link example: <https://localhost:7000/swagger/index.html>

When loading data to database overlapping data will be rewritten to new

Endpoints (parameters are provided as form-data):

### /cities/new

    - name - string, 
    - longtitude - double 
    - latitude - double, 
    - updateIfExists - (not required) set true to overrite existing city location

### /forecasts/new

    *At least one not required include parameter should be set to true*

    - cityName - string,
    - year - number,
    - month - number,
    - day - number,
    - additionalDays - (not required) provides number of days to add forecasts for after the main date (2023/01/01 + 3 additional days = 2023/01/01, 01/02, 01/03, 01/04)
    - includeTemperature - (not required) true to include temperature (°C)
    - includeWindSpeed - (not required) true to include wind speed (km/h)
    - includeRain - (not required) true to include rain data (mm)
    - includeSnowFall - (not required) true to include snow fall data (cm)
    - includeCloudCover - (not required) true to include cloud coverage (%)
