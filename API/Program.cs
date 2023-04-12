using API.Extensions;
using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly()); // SERVICIO DE AUTOMAPPER 

builder.Services.ConfigureRateLimitiong(); // SERVICIO DE RATELIMIT

builder.Services.ConfigureApiVersioning();
// Add services to the container.
builder.Services.ConfigureCors(); // ESTABLECIDO LOS CORS	

builder.Services.AddControllers(options =>
{
    // INSTRUCCION PARA PERMITIR EL FORMATO XML
    options.RespectBrowserAcceptHeader = true;

    // MENSAJE DE ERROR EN CASO DE QUE EL SERVIDOR NO SOPORTE EL FORMATO QUE EL CLIENTE SOLICITO
    options.ReturnHttpNotAcceptable = true; 

}).AddXmlSerializerFormatters();

//IMPLEMENTAMOS EL SERVICIO QUE NOS PERMITE USAR LOS REPOSITORIOS EN CUALQUIER COMPONENTE
builder.Services.AddAplicacionServices();

//INPLEMENTAMOS EL SERVICIO Y CONECCION DE MySql
builder.Services.AddDbContext<TiendaContext>(options =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// AGREGACION PARA EL MIDDLEWARE DE RATELIMIT 
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// APLICA CUALQUIER MIGRACIÓN QUE SE REALIZA DE MANER ASINCRONA A LA BASE DE DATOS
// TAMBIEN CREA LA BASE DE DATOS SI NO EXISTE
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();

        await TiendaContextSeed.SeedAsync(context, loggerFactory);

        await TiendaContextSeed.SeedRolesAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocurrió un error durante la migración");
    }
}



// AGREGACION DE LA POLITICA DE CORS
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
