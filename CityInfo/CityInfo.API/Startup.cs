using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            var connectionString = Startup.Configuration["connectionStrings:cityInfoDbConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif


            //.AddMvcOptions(o => o.OutputFormatters.Add(              - convert to XML output
            //    new XmlDataContractSerializerOutputFormatter()));

            //.AddJsonOptions(o =>                    - Revert to pascalcase json
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver
            //            as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            //loggerFactory.AddProvider(new NLogLoggerProvider()); - or we can use
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, CitiesWithoutPointsOfInterestDto>();
                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<PointsOfInterest, PointsOfInterestDto>();
                cfg.CreateMap<PointsOfInterestForCreationDto, PointsOfInterest>();
                cfg.CreateMap<UpdatePointOfInterestDto, PointsOfInterest>();
                cfg.CreateMap<PointsOfInterest, UpdatePointOfInterestDto>();
            });

            app.UseMvc();



            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
