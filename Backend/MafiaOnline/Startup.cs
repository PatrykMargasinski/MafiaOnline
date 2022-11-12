using AutoMapper;
using MafiaOnline.BusinessLogic;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Newtonsoft.Json.Serialization;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.ErrorHandling;
using MafiaOnline.BusinessLogic.Factories;
using MafiaAPI.Jobs;

namespace MafiaOnline
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
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //JSON Initializer
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                .Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver =
                new DefaultContractResolver());

            //Connection String
            services.AddDbContext<DataContext>(options =>
            options
            .UseSqlServer(Configuration.GetConnectionString("MafiaAppCon"))
            .UseSqlServer(b => b.MigrationsAssembly("MafiaOnline.DataAccess"))
            .EnableSensitiveDataLogging()
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IBossService, BossService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMissionService, MissionService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IHeadquartersService, HeadquartersService>();

            //Utils
            services.AddScoped<ISecurityUtils, SecurityUtils>();
            services.AddScoped<IMissionUtils, MissionUtils>();
            services.AddScoped<ITokenUtils, TokenUtils>();
            services.AddScoped<IBasicUtils, BasicUtils>();
            services.AddScoped<IReporter, Reporter>();
            services.AddScoped<IRandomizer, Randomizer>();
            services.AddScoped<IMapUtils, MapUtils>();

            //Factories
            services.AddScoped<IAgentFactory, AgentFactory>();
            services.AddScoped<IMissionFactory, MissionFactory>();

            //Validators
            services.AddScoped<IMissionValidator, MissionValidator>();
            services.AddScoped<IAgentValidator, AgentValidator>();
            services.AddScoped<IMessageValidator, MessageValidator>();
            services.AddScoped<IPlayerValidator, PlayerValidator>();

            //Job runners
            services.AddScoped<IPerformMissionJobRunner, PerformMissionJobRunner>();
            services.AddScoped<IAgentRefreshJobRunner, AgentRefreshJobRunner>();
            services.AddScoped<IMissionRefreshJobRunner, MissionRefreshJobRunner>();

            //Hosted service
            services.AddHostedService<MyHostedService>();

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile(
                    provider.CreateScope().ServiceProvider.GetService<IMissionUtils>(),
                    provider.CreateScope().ServiceProvider.GetService<ISecurityUtils>()
                    ));

            }).CreateMapper());
            
            services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "V1",
                    Title = "Sneaker API",
                    Description = "API for retrieving sneakers"
                });
            });

            services.AddLogging(loggingBuilder => {
                var loggingSection = Configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sneaker API V");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
