using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{
    public class MyHostedService : IHostedService
    {

        public MyHostedService(IServiceProvider services, ILogger<MyHostedService> logger, IHostApplicationLifetime appLifetime)
        {
            _services = services;
            _appLifetime = appLifetime;
            _logger = logger;
        }

        private readonly IServiceProvider _services;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<MyHostedService> _logger;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private async void OnStarted()
        {
            await RefreshJobs();
        }

        private async void OnStopping()
        {

        }

        private async void OnStopped()
        {

        }

        private async Task RefreshJobs()
        {
            using (var scope = _services.CreateScope())
            {
                IAgentService agentService =
                    scope.ServiceProvider
                        .GetService<IAgentService>();

                await agentService.ScheduleRefreshAgentsJob();

                IMissionService missionService =
                    scope.ServiceProvider
                        .GetService<IMissionService>();

                await missionService.ScheduleRefreshMissionsJob();
            }
        }
    }
}
