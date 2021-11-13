using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotosApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using PhotosApi.Data;

namespace PhotosApi
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
            /*Whenever a new context is requested, it will be returned from the context pool
              if it's available; Otherwise a new context will be created*/
            services.AddDbContextPool<MyDatabaseContext>(
                (options) => options.UseSqlServer (
                                Configuration.GetConnectionString("MyDatabaseConnection")
                             )
            );

            /* -According to ASP.NET Core docs, EF context (which is used inside PhotoDataAccess)
             *  should be added to the services container using the Scoped lifetime.
             *
             * -Registers the IPhotoDataAccess interface with the concrete type PhotoDataAccess, which is the Dependency
             *  Inversion Principle (DIP) primary concern (to ensure a class only depends upon higher-level abstractions)
             *  PS: Broadly speaking, DIP decreases coupling
             */
            services.AddScoped<IPhotoDataAccess, PhotoDataAccess>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotosApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhotosApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors((policy) => policy.WithOrigins("http://localhost:3000",
                                                       "http://photogallery.brazilsouth.azurecontainer.io",
                                                       "https://photogalleryr1.azurewebsites.net",
                                                       "https://photogalleryr2.azurewebsites.net",
                                                       "http://photogalleryr2.azurewebsites.net",
                                                       "http://photogallery-r2.brazilsouth.azurecontainer.io")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .AllowCredentials()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
