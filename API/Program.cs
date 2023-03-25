using API.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors(); // ESTABLECIDO LOS CORS	

builder.Services.AddControllers();

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
