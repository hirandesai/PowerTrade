using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Business.Services.Implementations;
using PowerTrade.Dto;
using PowerTrade.Infrastructure.HostedServices;
using PowerTrade.Services.Abstracts;
using PowerTrade.Services.Implementations;

namespace PowerTrade.Infrastructure.Configurations
{
    public static class ServiceConfiguration
    {
        public static void AddAppServices(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
            serviceProvider.AddHostedService<SchedulerBackgroundService>();

            AddBusinessServices(serviceProvider, configuration);
            AddServices(serviceProvider);
        }

        private static void AddBusinessServices(IServiceCollection serviceProvider, IConfiguration configuration)
        {
            serviceProvider.Configure<IntraDayReportConfig>(configuration.GetSection(nameof(IntraDayReportConfig)));
            serviceProvider.AddScoped<IIntraDayReportScheduler>(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<IntraDayReportScheduler>>();
                var intraDayReportConfig = serviceProvider.GetService<IOptions<IntraDayReportConfig>>();
                var queueService = serviceProvider.GetService<IQueueService<IntraDaySchedule>>();
                var dateTimeProvider = serviceProvider.GetService<IDateTimeProvieder>();

                if (intraDayReportConfig == null)
                    throw new ArgumentNullException($"Missing config {nameof(IntraDayReportConfig)}");

                return new IntraDayReportScheduler(logger,
                                                    queueService,
                                                    dateTimeProvider,
                                                    new IntraDayReportSchedulerConfig(intraDayReportConfig.Value.SchedulerFrequencyInSec));
            });
        }

        private static void AddServices(IServiceCollection serviceProvider)
        {
            serviceProvider.AddScoped(typeof(IQueueService<>), typeof(QueueService<>));
            serviceProvider.AddScoped<IDateTimeProvieder, DateTimeProvieder>();
        }

    }
}
