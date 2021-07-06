using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NasaAstronomyPicture.Api.Models;
using NasaAstronomyPicture.Api.Services;

namespace NasaAstronomyPicture.Api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NasaAstronomyContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                , optionBuilder => optionBuilder.MigrationsAssembly(typeof(NasaAstronomyContext).Assembly.FullName))
            );

            services.AddScoped<INasaAstronomyService, NasaAstronomyService>();
        }
    }
}