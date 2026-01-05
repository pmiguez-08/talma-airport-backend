namespace TalmaAirport.Api.Models; 

public class Airline 
{ 
    public int AirlineId { get; set; } //  define el id de la aerolinea
    public string Name { get; set; } = string.Empty; //  define el nombre de la aerolinea
    public string Code { get; set; } = string.Empty; //  define el codigo de la aerolinea
    public List<Flight> Flights { get; set; } = new(); //  define la lista de vuelos asociados
} 

