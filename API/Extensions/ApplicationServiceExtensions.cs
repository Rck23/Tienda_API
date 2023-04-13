using API.Helpers;
using API.Services;
using AspNetCoreRateLimit;
using Core.Entities;
using Core.Intefaces;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        // AGREGACION DE SERVICIOS DE USUARIOS
        services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
        services.AddScoped<IUserService, UserService>();
    }

    // NUEVO METODO DE EXTENSIONES
    public static void ConfigureRateLimitiong(this IServiceCollection services)
    {
        // CONFIGURACION PARA LIMITAR LAS PETICIONES POR IP

        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    // REGLAS DE LIMITE 
                    new RateLimitRule
                    {
                        Endpoint ="*",
                        Period = "10s",
                        Limit =2
                    }
                };
            });
    }

    // METODO DE EXTENSION DE VERSIONADO 
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // AGREGA LA VERSION DE LA API
            options.DefaultApiVersion = new ApiVersion(1, 0);

            options.AssumeDefaultVersionWhenUnspecified = true;

            // LEER OTRA VERSION DE LA API POR QUERY STRING
            // options.ApiVersionReader = new QueryStringApiVersionReader("ver");

            // LEER OTRA VERSION DE LA API POR ENCABEZADO 
            // options.ApiVersionReader = new HeaderApiVersionReader("X-Version");

            //COMBINAR LOS METODOS DE VERSIONADO 
            options.ApiVersionReader = ApiVersionReader.Combine(
                 new QueryStringApiVersionReader("ver"),
                 new HeaderApiVersionReader("X-Version"));

            // VER QUE VERSIONES SOPORTA 
            options.ReportApiVersions = true;


        });
    }

    // METODO PARA TOKEN
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        //Configuration from AppSettings
        services.Configure<JWT>(configuration.GetSection("JWT"));

        //Adding Athentication - JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false; // Esto activa y desactiva HTTPS
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
    }

}
