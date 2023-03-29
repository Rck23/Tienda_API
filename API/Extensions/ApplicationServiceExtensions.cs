using Core.Intefaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;

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

    // IMPLEMENTACION DE TODOS LOS REPOSITORIOS
    public static void AddAplicacionServices(this IServiceCollection services)
    {
             // REPOSITORIOS DE FORMA MANUAL
        //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        //services.AddScoped<IProductoReposity, ProductoRepository>(); 
        //services.AddScoped<IMarcaReposity, MarcaReposity>();
        //services.AddScoped<ICategoriaReposity, CategoriaReposity>();

        // AQUI YA CONTIENE TODOS LOS REPOSITORIOS
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
