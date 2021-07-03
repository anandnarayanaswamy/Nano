using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NasaAstronomyPicture.Api.Models;

namespace NasaAstronomyPicture.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NasaAstronomyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                   ,optionBuilder => optionBuilder.MigrationsAssembly(typeof(NasaAstronomyContext).Assembly.FullName))
                );

            services.AddHttpClient();

            services.AddHttpClient("meta", c=>
            {
                c.BaseAddress = new System.Uri("https://api.nasa.gov/planetary/");
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
