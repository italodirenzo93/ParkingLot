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
            services.AddDbContext<VehiklParkingDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("MainDb")));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000"));
            });
            
            // Add services
            services.AddScoped<ITicketService, TicketService>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {     
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    
            }
            
            app.UseCors();
            app.UseMvc();
        }
    }
}
