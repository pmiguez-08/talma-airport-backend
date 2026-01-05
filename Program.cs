using Microsoft.EntityFrameworkCore; 
using TalmaAirport.Api.Data; 

var builder = WebApplication.CreateBuilder(args); //  crea el constructor de la aplicacion

builder.Services.AddControllers(); // habilita los controladores para exponer endpoints

builder.Services.AddEndpointsApiExplorer(); //  habilita la exploracion de endpoints para Swagger

builder.Services.AddSwaggerGen(); // genera la documentacion Swagger

builder.Services.AddCors(options => //  configura politicas de CORS
{
    options.AddPolicy("AllowFrontend", policy => //  crea una politica llamada AllowFrontend
    {
        policy.WithOrigins("http://localhost:5173") //  permite llamadas desde el frontend
              .AllowAnyHeader() // permite cualquier header
              .AllowAnyMethod(); //permite cualquier metodo HTTP
    }); 
}); 

builder.Services.AddDbContext<AppDbContext>(options => // registra el contexto de base de datos
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //  configura SQL Server usando la cadena de conexion
}); 

var app = builder.Build(); // construye la aplicacion

if (app.Environment.IsDevelopment()) // verifica si el ambiente es desarrollo
{
    app.UseSwagger(); //  habilita el json de Swagger

    app.UseSwaggerUI(); //  habilita la interfaz web de Swagger
} 

app.UseHttpsRedirection(); //redirige de http a https si aplica

app.UseCors("AllowFrontend"); // aplica la politica CORS al pipeline

app.MapControllers(); // mapea las rutas de los controladores

app.Run(); // inicia la aplicacion
