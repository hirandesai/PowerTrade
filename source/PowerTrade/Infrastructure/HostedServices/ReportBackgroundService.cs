using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerTrade.Business.Services.Abstracts;

namespace PowerTrade.Infrastructure.HostedServices
{
    public class ReportBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ReportBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var scheduler = scope.ServiceProvider.GetService<IIntraDayReportScheduleProcessor>();
            await scheduler.Start(stoppingToken);
        }
    }
}
