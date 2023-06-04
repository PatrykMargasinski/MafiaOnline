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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using MafiaOnline.DataAccess.Helpers;

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
            .UseLazyLoadingProxies()
            .EnableSensitiveDataLogging()
            );

            services.AddDefaultIdentity<Player>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<Player>>("emailconfirmation");

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IBossService, BossService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMissionService, MissionService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IHeadquartersService, HeadquartersService>();
            services.AddScoped<IAmbushService, AmbushService>();
            services.AddScoped<INewsService, NewsService>();

            //Utils
            services.AddScoped<ISecurityUtils, SecurityUtils>();
            services.AddScoped<IMissionUtils, MissionUtils>();
            services.AddScoped<ITokenUtils, TokenUtils>();
            services.AddScoped<IBasicUtils, BasicUtils>();
            services.AddScoped<IReporter, Reporter>();
            services.AddScoped<IRandomizer, Randomizer>();
            services.AddScoped<IMapUtils, MapUtils>();
            services.AddScoped<IMovingAgentUtils, MovingAgentUtils>();
            services.AddScoped<IAgentUtils, AgentUtils>();
            services.AddScoped<IPlayerUtils, PlayerUtils>();
            services.AddScoped<IMailSender, MailSender>();
            services.AddScoped<IGameUtils, GameUtils>();

            //Factories
            services.AddScoped<IAgentFactory, AgentFactory>();
            services.AddScoped<IMissionFactory, MissionFactory>();
            services.AddScoped<IAmbushFactory, AmbushFactory>();

            //Validators
            services.AddScoped<IMissionValidator, MissionValidator>();
            services.AddScoped<IAgentValidator, AgentValidator>();
            services.AddScoped<IMessageValidator, MessageValidator>();
            services.AddScoped<IPlayerValidator, PlayerValidator>();
            services.AddScoped<IAmbushValidator, AmbushValidator>();
            services.AddScoped<INewsValidator, NewsValidator>();

            //Job runners
            services.AddScoped<IPerformMissionJobRunner, PerformMissionJobRunner>();
            services.AddScoped<IAgentRefreshJobRunner, AgentRefreshJobRunner>();
            services.AddScoped<IMissionRefreshJobRunner, MissionRefreshJobRunner>();
            services.AddScoped<IAgentMovingOnMissionJobRunner, AgentMovingOnMissionJobRunner>();
            services.AddScoped<IArrangeAmbushJobRunner, ArrangeAmbushJobRunner>();
            services.AddScoped<IPatrolJobRunner, PatrolJobRunner>();
            services.AddScoped<IReturnWithLootJobRunner, ReturnWithLootJobRunner>();
            services.AddScoped<IAttackAmbushJobRunner, AttackAmbushJobRunner>();


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

            // Routing
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });



            //Security
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Security:AuthKey")))
                    };
                });

            //Logging
            services.AddLogging(loggingBuilder => {
                var loggingSection = Configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
 

            app.UseRouting();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
