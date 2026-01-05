namespace TalmaAirport.Api.Models; 

public class City 
{ 
    public int CityId { get; set; } //define el id de la ciudad
    public string Name { get; set; } = string.Empty; // define el nombre de la ciudad
    public string Code { get; set; } = string.Empty; //  define el codigo de la ciudad
    public List<Flight> OriginFlights { get; set; } = new(); //  define vuelos donde la ciudad es origen
    public List<Flight> DestinationFlights { get; set; } = new(); // define vuelos donde la ciudad es destino
} 
