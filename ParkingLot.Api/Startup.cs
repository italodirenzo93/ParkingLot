using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ParkingLot.Data;
using ParkingLot.Tickets;

namespace ParkingLot.Api
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
            // Add Cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Parking Lot API", Version = "v1"});
                options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
                options.MapType<TimeSpan?>(() => new OpenApiSchema
                {
                    Type = "number",
                    Default = null,
                    Description = "The timestamp encoded in microseconds"
                });
            });

            // Add services
            services.AddDbContext<ParkingLotDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ParkingLotDb")));
            services.AddSingleton(Configuration.GetSection("ParkingLot").Get<ParkingLotConfig>());
            services.AddScoped<ITicketService, TicketService>();

            // Add other necessities
            services.AddControllers();
            services.AddRouting(options => { options.LowercaseUrls = true; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Lot API v1"); });

            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(options => { options.MapControllers(); });
        }
    }
}
