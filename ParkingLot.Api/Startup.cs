using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            // Configure the database connection. Could also use other backends such as SQLite, MySQL, SQLServer, etc.
            // Selected in-memory database for simplicity of demonstration
            services.AddDbContext<ParkingLotDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ParkingLotDb")));
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000"));
            });

            // Config objects
            services.AddSingleton(Configuration.GetSection("ParkingLot").Get<ParkingLotConfig>());
            
            // Add services
            services.AddScoped<ITicketService, TicketService>();

            services.AddControllers();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {     
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    
            }
            
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(options => { options.MapControllers(); });
        }
    }
}
