using Microsoft.Extensions.Hosting;

namespace PowerTrade.Infrastructure.HostedServices
{
    public class SchedulerBackgroundService : BackgroundService
    {
        public SchedulerBackgroundService()
        {
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }
    }
}
