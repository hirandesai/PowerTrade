using System.Security;
using Axpo;
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
            serviceProvider.AddHostedService<ReportBackgroundService>();

            AddBusinessServices(serviceProvider, configuration);
            AddServices(serviceProvider);
        }

        private static void AddBusinessServices(IServiceCollection serviceProvider, IConfiguration configuration)
        {
            serviceProvider.Configure<IntraDayReportConfig>(configuration.GetSection(nameof(IntraDayReportConfig)));
            serviceProvider.AddScoped<IIntraDayReportScheduler>(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<IntraDayReportScheduler>>();
                var queueService = serviceProvider.GetService<IQueueService<IntraDaySchedule>>();
                var dateTimeProvider = serviceProvider.GetService<IDateTimeProvieder>();
                var intraDayReportConfig = GetIntraDayReportConfig(serviceProvider);
                return new IntraDayReportScheduler(logger,
                                                    queueService,
                                                    dateTimeProvider,
                                                    new IntraDayReportSchedulerConfig(intraDayReportConfig.SchedulerFrequencyInSec));
            });

            serviceProvider.AddScoped<IIntraDayReportCsvWriter>(serviceProvider =>
            {
                var fileService = serviceProvider.GetService<IFileService>();
                var intraDayReportConfig = GetIntraDayReportConfig(serviceProvider);

                return new IntraDayReportCsvWriter(fileService, new IntraDayReportCsvConfig(intraDayReportConfig.ReportPath, intraDayReportConfig.CsvDelimiter));
            });

            serviceProvider.AddScoped<IIntraDayReportScheduleProcessor>(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<IntraDayReportScheduleProcessor>>();
                var queueService = serviceProvider.GetService<IQueueService<IntraDaySchedule>>();
                var powerServiceClient = serviceProvider.GetService<IPowerServiceClient>();
                var intraDayReportCsvWriter = serviceProvider.GetService<IIntraDayReportCsvWriter>();
                var intraDayReportConfig = GetIntraDayReportConfig(serviceProvider);

                return new IntraDayReportScheduleProcessor(logger,
                                                    queueService,
                                                    powerServiceClient,
                                                    intraDayReportCsvWriter,
                                                    new IntraDayReportScheduleProcessorConfig(intraDayReportConfig.LocalTimezoneId));
            });
        }

        private static void AddServices(IServiceCollection serviceProvider)
        {
            serviceProvider.AddSingleton(typeof(IQueueService<>), typeof(QueueService<>));
            serviceProvider.AddScoped<IDateTimeProvieder, DateTimeProvieder>();
            serviceProvider.AddScoped<IPowerServiceClient, PowerServiceClient>();
            serviceProvider.AddScoped<IPowerService, PowerService>();
            serviceProvider.AddScoped<IFileService, FileService>();
        }

        private static IntraDayReportConfig GetIntraDayReportConfig(IServiceProvider serviceProvider)
        {
            var intraDayReportConfig = serviceProvider.GetService<IOptions<IntraDayReportConfig>>();

            if (intraDayReportConfig == null)
                throw new ArgumentNullException($"Missing config {nameof(IntraDayReportConfig)}");

            if (intraDayReportConfig.Value.SchedulerFrequencyInSec == 0)
                throw new ArgumentException($"{nameof(IntraDayReportConfig.SchedulerFrequencyInSec)} cannot be 0.");

            if (string.IsNullOrEmpty(intraDayReportConfig.Value.ReportPath) || !Directory.Exists(intraDayReportConfig.Value.ReportPath))
                throw new ArgumentNullException($"{nameof(IntraDayReportConfig.ReportPath)} cannot be empty or directory should exists.");

            if (string.IsNullOrEmpty(intraDayReportConfig.Value.CsvDelimiter))
                throw new ArgumentNullException($"{nameof(IntraDayReportConfig.CsvDelimiter)} cannot be empty.");

            if (string.IsNullOrEmpty(intraDayReportConfig.Value.LocalTimezoneId))
                throw new ArgumentNullException($"{nameof(IntraDayReportConfig.LocalTimezoneId)} cannot be empty.");

            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(intraDayReportConfig.Value.LocalTimezoneId);
            }
            catch (Exception ex)
            {
                if (ex is TimeZoneNotFoundException || ex is InvalidTimeZoneException || ex is SecurityException)
                    throw new ArgumentException($"{nameof(IntraDayReportConfig.LocalTimezoneId)} ({intraDayReportConfig.Value.LocalTimezoneId}) is not valid.");

                throw;
            }
            return intraDayReportConfig.Value;
        }
    }
}
