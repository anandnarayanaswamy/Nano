using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NasaAstronomyPicture.Api.Installers
{
    public class CORSInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
