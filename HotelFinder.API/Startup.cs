using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using HotelFinder.Business.Abstract;
using HotelFinder.Business.Concrete;
using HotelFinder.DataAccess.Abstract;
using HotelFinder.DataAccess.Contcrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelFinder.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IHotelService, HotelManager>();
            services.AddSingleton<IHotelRepository, HotelRepository>();
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = (doc =>
                  {
                      doc.Info.Title = "All Hotels Api";
                      doc.Info.Version = "1.0.13";
                      doc.Info.Contact = new NSwag.OpenApiContact()
                      {
                          Name = "Mete CAPAR",
                          Url = "http://capar.net",
                          Email = "mete@capar.net"
                      };
                  });
            });

            services.AddHealthChecks().AddSqlServer(Configuration.GetConnectionString("HotelDB"));

           

            services.AddHealthChecksUI().AddInMemoryStorage();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
           
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = registration => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
        }
    }
}
