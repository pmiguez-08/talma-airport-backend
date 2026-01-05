using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using TalmaAirport.Api.Data; 
using TalmaAirport.Api.Models; 

namespace TalmaAirport.Api.Controllers; 

[ApiController] // marca la clase como un controlador de API
[Route("api/[controller]")] //  define la ruta base api flights
public class FlightsController : ControllerBase // define el controlador de vuelos
{ 
    private readonly AppDbContext _db; 

    public FlightsController(AppDbContext db) //recibe el contexto por inyeccion de dependencias
    { 
        _db = db; // asigna el contexto recibido a la variable privada
    } 

    [HttpGet] 
    public async Task<ActionResult<object>> Search( //  define un metodo de busqueda con filtros
        [FromQuery] string? airline, // 
        [FromQuery] string? flightNumber, //  recibe el filtro de numero de vuelo
        [FromQuery] string? originCity, //  recibe el filtro de ciudad origen
        [FromQuery] string? destinationCity, //  recibe el filtro de ciudad destino
        [FromQuery] DateOnly? date, //  recibe el filtro de fecha
        [FromQuery] int page = 1, //  recibe la pagina solicitada
        [FromQuery] int pageSize = 10) //  recibe el tamaño de pagina
    { 
        if (page < 1) page = 1; //  asegura que la pagina sea minimo 1
        if (pageSize < 1) pageSize = 10; //  asegura que el tamaño de pagina sea minimo 1
        if (pageSize > 50) pageSize = 50; // limita el tamaño de pagina para evitar cargas grandes

        var query = _db.Flights //  inicia la consulta desde la tabla Flights
            .AsNoTracking() //  mejora rendimiento porque solo leemos
            .Include(f => f.Airline) //  carga la aerolinea relacionada
            .Include(f => f.OriginCity) //  carga la ciudad de origen relacionada
            .Include(f => f.DestinationCity) // carga la ciudad de destino relacionada
            .AsQueryable(); // permite agregar filtros despues

        if (!string.IsNullOrWhiteSpace(airline)) //  valida si enviaron filtro de aerolinea
            query = query.Where(f => f.Airline.Name.Contains(airline.Trim())); //  filtra por nombre de aerolinea

        if (!string.IsNullOrWhiteSpace(flightNumber)) //  valida si enviaron filtro de numero
            query = query.Where(f => f.FlightNumber.Contains(flightNumber.Trim())); //  filtra por numero de vuelo

        if (!string.IsNullOrWhiteSpace(originCity)) // valida si enviaron filtro de ciudad origen
            query = query.Where(f => f.OriginCity.Name.Contains(originCity.Trim())); //  filtra por ciudad origen

        if (!string.IsNullOrWhiteSpace(destinationCity)) //  valida si enviaron filtro de ciudad destino
            query = query.Where(f => f.DestinationCity.Name.Contains(destinationCity.Trim()));

        if (date.HasValue) //  valida si enviaron filtro de fecha
            query = query.Where(f => f.FlightDate == date.Value); //  filtra por fecha

        var totalItems = await query.CountAsync(); // cuenta el total de elementos filtrados
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); //  calcula el total de paginas

        var items = await query //  toma la consulta final
            .OrderByDescending(f => f.FlightId) //  ordena por id de vuelo descendente
            .Skip((page - 1) * pageSize) //  salta registros segun la pagina
            .Take(pageSize) //  toma los registros segun el tamaño de pagina
            .Select(f => new //  crea una respuesta simple para el frontend
            { 
                id = f.FlightId, // expone el id del vuelo
                airline = f.Airline.Name, //  expone el nombre de la aerolinea
                flightNumber = f.FlightNumber, //  expone el numero del vuelo
                originCity = f.OriginCity.Name, //  expone el nombre de la ciudad origen
                destinationCity = f.DestinationCity.Name, // Esto expone el nombre de la ciudad destino
                date = f.FlightDate //  expone la fecha del vuelo
            })
            .ToListAsync(); //  ejecuta la consulta y la convierte en lista

        var result = new 
        {
            page = page, //  devuelve la pagina actual
            pageSize = pageSize, //  devuelve el tamaño de pagina
            totalItems = totalItems, //  devuelve el total de elementos
            totalPages = totalPages, //  devuelve el total de paginas
            items = items //  devuelve los vuelos encontrados
        }; 

        return Ok(result); //  retorna el resultado al cliente
    } 

    [HttpGet("{id:int}")] // indica que este metodo responde a GET api flights id
    public async Task<ActionResult<object>> GetById(int id) //  define un metodo para obtener un vuelo por id
    { 
        var flight = await _db.Flights 
            .AsNoTracking() //  mejora rendimiento porque solo leemos
            .Include(f => f.Airline) // carga la aerolinea relacionada
            .Include(f => f.OriginCity) //  carga la ciudad origen relacionada
            .Include(f => f.DestinationCity) //  carga la ciudad destino relacionada
            .FirstOrDefaultAsync(f => f.FlightId == id); // Esto busca el vuelo por id

        if (flight == null) //  valida si no se encontro el vuelo
            return NotFound(new { message = "Flight not found" }); //  retorna 404 si no existe

        var result = new 
        { 
            id = flight.FlightId, //  expone el id del vuelo
            airline = flight.Airline.Name, //  expone la aerolinea
            flightNumber = flight.FlightNumber, //  expone el numero
            originCity = flight.OriginCity.Name, // expone la ciudad origen
            destinationCity = flight.DestinationCity.Name, // expone la ciudad destino
            date = flight.FlightDate //  expone la fecha
        }; 

        return Ok(result); 
    } 
} 
