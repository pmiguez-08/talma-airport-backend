namespace TalmaAirport.Api.Models; 

public class Flight 
{ 
    public int FlightId { get; set; } //   el id del vuelo
    public int AirlineId { get; set; } //  define la llave foranea hacia Airline
    public int OriginCityId { get; set; } //  llave foranea hacia City para origen
    public int DestinationCityId { get; set; } // llave foranea hacia City para destino
    public string FlightNumber { get; set; } = string.Empty; //  numero de vuelo
    public DateOnly FlightDate { get; set; } // fecha del vuelo
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // fecha de creacion del registro
    public Airline Airline { get; set; } = null!; // navegacion hacia Airline
    public City OriginCity { get; set; } = null!; // navegacion hacia City de origen
    public City DestinationCity { get; set; } = null!; // navegacion hacia City de destino
} 

