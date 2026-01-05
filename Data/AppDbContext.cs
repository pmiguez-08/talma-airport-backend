using Microsoft.EntityFrameworkCore; // Esto importa EF Core
using TalmaAirport.Api.Models; // Esto importa los modelos

namespace TalmaAirport.Api.Data; // Esto define el espacio de nombres

public class AppDbContext : DbContext // Esto define el contexto de base de datos
{ // Esto abre la clase
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Esto configura el contexto con opciones

    public DbSet<Airline> Airlines => Set<Airline>(); // Esto expone la tabla de aerolineas
    public DbSet<City> Cities => Set<City>(); // Esto expone la tabla de ciudades
    public DbSet<Flight> Flights => Set<Flight>(); // Esto expone la tabla de vuelos

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Esto permite configurar reglas del modelo
    { // Esto abre el metodo
        modelBuilder.Entity<Airline>().HasIndex(a => a.Code).IsUnique(); // Esto hace unico el codigo de aerolinea
        modelBuilder.Entity<City>().HasIndex(c => c.Code).IsUnique(); // Esto hace unico el codigo de ciudad

        modelBuilder.Entity<Flight>().HasIndex(f => new { f.AirlineId, f.FlightNumber, f.FlightDate }).IsUnique(); // Esto hace unico aerolinea numero y fecha
        modelBuilder.Entity<Flight>().HasCheckConstraint("CK_Flights_DifferentCities", "[OriginCityId] <> [DestinationCityId]"); // Esto evita origen igual a destino

        modelBuilder.Entity<Flight>().HasOne(f => f.Airline).WithMany(a => a.Flights).HasForeignKey(f => f.AirlineId); // Esto configura relacion vuelo aerolinea
        modelBuilder.Entity<Flight>().HasOne(f => f.OriginCity).WithMany(c => c.OriginFlights).HasForeignKey(f => f.OriginCityId).OnDelete(DeleteBehavior.Restrict); // Esto configura relacion vuelo ciudad origen
        modelBuilder.Entity<Flight>().HasOne(f => f.DestinationCity).WithMany(c => c.DestinationFlights).HasForeignKey(f => f.DestinationCityId).OnDelete(DeleteBehavior.Restrict); // Esto configura relacion vuelo ciudad destino

        base.OnModelCreating(modelBuilder); // Esto llama la configuracion base
    } // Esto cierra el metodo
} // Esto cierra la clase
