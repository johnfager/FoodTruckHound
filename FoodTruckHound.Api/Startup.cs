using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Data.Configuration;
using FoodTruckHound.Data.Repositories;
using FoodTruckHound.Data.Services;
using FoodTruckHound.Data.Services.SfGov;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FoodTruckHound.Api
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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.Converters.Add(new StringEnumConverter());
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 options.UseCamelCasing(true);
             });

            services.AddLogging();

            services.AddScoped<IFoodTruckDataService, SfGovMobileFoodScheduleService>();
            services.AddScoped<IFoodTruckLookupRepository, FoodTruckLookupInMemoryRepository>();
            services.AddScoped<IFoodTruckSpatialService, FoodTruckSpatialService>();

            // endpoint configuration
            services.Configure<SfGovEndpoints>(Configuration.GetSection(
                                      nameof(SfGovEndpoints)));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
