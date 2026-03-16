using AuthService.Domain.Entities;
using AuthService.Domain.Constants;
using AuthService.Persistence.Data;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Interfaces;
using AuthService.Application.Service;
using AuthService.Persistence.Repositories;

 
namespace AuthService.Api.Extensions;
 
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // INICIALIZANDO EL CONEXION A LA BASE DE DATOS
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                       .UseSnakeCaseNamingConvention());

        // Configure application services <------ ACTUALIZACIÓN
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAuthService, Application.Service.AuthService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
       
        // Se registra el servicio de inicialización de datos (DataSeeder) en el contenedor de servicios con un alcance transitorio (Transient), lo que significa que se creará una nueva instancia del servicio cada vez que se solicite.
        services.AddScoped<IEmailService, EmailService>();
       
        services.AddHealthChecks();
 
        return services;
    }
public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    return services;

    }
}
 