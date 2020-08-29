using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MikeyFriedChicken.WebAPI.FireForgetRepository.Database;

namespace MikeyFriedChicken.WebAPI.FireForgetRepository
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
            services.AddControllers();
            services.AddDbContext<BloggingContext>();
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFireForgetRepositoryHandler, FireForgetRepositoryHandler>();
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi(options => { });
            app.UseSwaggerUi3(options => { });
            app.UseReDoc(options => { });
        }
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection collection)
        {
            collection.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Fire and Forget Repository";
                    document.Info.Description = "Example project for running database commands from controllers on seperate threads";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Michael",
                        Email = string.Empty,
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                    };
                };

            });


            return collection;
        }
    }
}
