using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NasaAstronomyPicture.Api.Installers
{
    public class JSONOptionInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.PropertyNamingPolicy = null;
                option.JsonSerializerOptions.WriteIndented = true;
                option.JsonSerializerOptions.IgnoreNullValues = true;
                option.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

            });
        }
    }
}
