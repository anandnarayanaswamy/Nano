using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace NasaAstronomyPicture.Api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {


            services.AddHttpClient();

            services.AddHttpClient("meta", c =>
            {
                c.BaseAddress = new System.Uri("https://api.nasa.gov/planetary/");
            });

            services.AddMvc();

            services.AddControllers();

            
        }
    }
}