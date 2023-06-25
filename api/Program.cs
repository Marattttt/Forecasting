using Microsoft.EntityFrameworkCore;

using WeatherForecast.Data;
using WeatherForecast.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<LocalForecastService>();
builder.Services.AddScoped<OpenMeteoCommunicationService>();

builder.Services.AddDbContext<WeatherForecastContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("WeatherForecast"));
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
