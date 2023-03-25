namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        //PERMITIMOS ACCESO DESDE CUALCUQUIER ORIGEN, ACCESO Y ENCABEZADO
        //SOLO APLICA PARA DESARROLLO (EN PRODUCCION HAY QUE ESPECIFICAR TODO!)
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin() // Dominios (http://localhost:3000, http://Pagina.com)
                .AllowAnyMethod() // metodos como GET, POST
                .AllowAnyHeader()); // Aceptacion de "Content-Type", "Accept"
        });
}
