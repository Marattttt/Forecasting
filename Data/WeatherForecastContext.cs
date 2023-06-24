using Microsoft.EntityFrameworkCore;

using WeatherForecast.Models;

namespace WeatherForecast.Data;

public partial class WeatherForecastContext : DbContext
{
    public WeatherForecastContext(DbContextOptions<WeatherForecastContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<Forecast> Forecasts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("cities_pkey");

            entity.ToTable("cities");

            entity.Property(e => e.CityId)
                .ValueGeneratedOnAdd()
                .HasColumnName("city_id");
                
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.Location)
                .HasColumnName("location");
        });

        modelBuilder.Entity<Forecast>(entity =>
        {
            entity.HasKey(e => new { e.CityId, e.ForecastDate }).HasName("forecasts_pkey");

            entity.ToTable("forecasts");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.ForecastDate).HasColumnName("forecast_date");
            entity.Property(e => e.CloudCover).HasColumnName("cloud_cover");
            entity.Property(e => e.Rain).HasColumnName("rain");
            entity.Property(e => e.Snowfall).HasColumnName("snowfall");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.WindSpeed).HasColumnName("wind_speed");

            entity.HasOne(d => d.City).WithMany(p => p.Forecasts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("fk_cities");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
