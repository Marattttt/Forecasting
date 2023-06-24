using Microsoft.EntityFrameworkCore;

using WeatherForecast.Data;
using WeatherForecast.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DbForecastService>();
builder.Services.AddScoped<CommunicationService>();

builder.Services.AddDbContext<WeatherForecastContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("WeatherForecast"));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WeatherForecastContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
