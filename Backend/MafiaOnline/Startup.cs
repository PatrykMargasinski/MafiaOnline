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

            //Connection String
            services.AddDbContext<DataContext>(options =>
            options
            .UseSqlServer(Configuration.GetConnectionString("MafiaAppCon"))
            .UseSqlServer(b => b.MigrationsAssembly("MafiaOnline.DataAccess"))
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IBossService, BossService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMissionService, MissionService>();

            //Utils
            services.AddScoped<ISecurityUtils, SecurityUtils>();
            services.AddScoped<IMissionUtils, MissionUtils>();
            services.AddScoped<ITokenUtils, TokenUtils>();
            services.AddScoped<IBasicUtils, BasicUtils>();

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile(provider.CreateScope().ServiceProvider.GetService<IMissionUtils>()));

            }).CreateMapper());

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
