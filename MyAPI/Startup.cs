using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAPI.Database;

namespace MyAPI
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddControllers();
            //*Config Services
            //* it  dependon SQL
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection_azure_sql_eage")));

            //Create database for code frist di  injection database
            var servicesProvider = services.BuildServiceProvider();
            DatabaseInit.INIT(servicesProvider);
            
            // Setup CORS
            services.AddCors(options =>
            {
                //* Allow Services name of rule AllowAll
                // Setup CORS
                options.AddPolicy("AllowAll", builder => 
                                builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());

                // options.AddPolicy("AllowSpecific", builder =>
                // {
                //     builder.WithOrigins("http://localhost:4848", "http://www.codemobiles.com")
                //            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                // });

                // options.AddPolicy("AllowSpecificMethods", builder =>
                // {
                //     builder.WithOrigins("http://localhost:4848", "https://www.w3schools.com")
                //            .WithMethods("POST", "PUT")
                //            .AllowAnyHeader().AllowCredentials();
                // });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Allow all method
            app.UseCors();

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
        }
    }
}
