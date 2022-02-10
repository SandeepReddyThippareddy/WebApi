using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebAPI.BLL.Core.IRepositories;
using WebAPI.BLL.Data;

namespace WebAPI.ServiceExtensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureSqlServerConnection(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<WebApiContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
             services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaikiAPI", Version = "v1" });
            });
        }

        public static void ConfigureRepositoryPattern(this IServiceCollection services)
        {
             services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }

}