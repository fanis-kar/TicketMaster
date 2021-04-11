using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketMaster.Business.Interfaces;
using TicketMaster.Business.Services;
using TicketMaster.Data.Context;
using TicketMaster.Data.Interfaces;
using TicketMaster.Api.Errors;
using TicketMaster.Api.Filter;

namespace TicketMaster
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

            services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("TicketMasterDatabase")));//, ServiceLifetime.Transient);            
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped(typeof(IMyDbContextWrapper), typeof(MyDbContextWrapper));           
            services.AddScoped(typeof(IService<>), typeof(Service<>));           
            services.AddScoped(typeof(ITicketService), typeof(TicketService));                    
            services.AddScoped(typeof(IShowService), typeof(ShowService));
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options =>
                                {
                                    options.AllowEmptyInputInBodyModelBinding = true;
                                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
            }
            */
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHsts();

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
