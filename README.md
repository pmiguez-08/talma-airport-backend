# TalmaAirport.Api

API REST para consulta de vuelos por aerolínea, fecha, ciudad y número de vuelo

## Requisitos
- .NET SDK 8
- SQL Server
- Node no es necesario para el backend

## Configuración
Editar appsettings.json y configurar la cadena de conexión en DefaultConnection

## Ejecutar
En la carpeta del proyecto:

dotnet restore  
dotnet run  

La API se expone en http://localhost:5147

Swagger queda disponible en:
http://localhost:5147/swagger

## Endpoints
GET /api/flights  
GET /api/flights?airline=  
GET /api/flights?flightNumber=  
GET /api/flights?originCity=  
GET /api/flights?destinationCity=  
GET /api/flights?date=YYYY-MM-DD  
GET /api/flights/{id}

## Base de datos
Migraciones con herramienta local:

dotnet tool restore  
dotnet tool run dotnet-ef migrations add InitialDb  
dotnet tool run dotnet-ef database update  