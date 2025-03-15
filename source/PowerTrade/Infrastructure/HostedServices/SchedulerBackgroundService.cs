using Microsoft.Extensions.Hosting;
using PowerTrade.Business.Services.Abstracts;

namespace PowerTrade.Infrastructure.HostedServices
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly IIntraDayReportScheduler scheduler;

        public SchedulerBackgroundService(IIntraDayReportScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await scheduler.Start(stoppingToken);
    }
}
