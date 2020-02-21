using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            // Configure the database connection
            services.AddDbContext<ParkingLotDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ParkingLotDb")));

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

            // Config objects
            services.AddSingleton(Configuration.GetSection("ParkingLot").Get<ParkingLotConfig>());

            // Add services
            services.AddScoped<ITicketService, TicketService>();

            // Add other necessities
            services.AddControllers();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Run migrations on startup
            var context = app.ApplicationServices.GetRequiredService<ParkingLotDbContext>();
            context.Database.Migrate();

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
