using Microsoft.Extensions.Logging;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Business.Services.Extensions;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduleProcessor : IIntraDayReportScheduleProcessor
    {
        private const int DelayTimeWhenScheduleIsNotFound = 3;
        private readonly ILogger<IntraDayReportScheduler> logger;
        private readonly IQueueService<IntraDaySchedule> queueService;
        private readonly IPowerServiceClient powerServiceClient;
        private readonly IIntraDayReportCsvWriter intraDayReportCsvWriter;
        private readonly IntraDayReportScheduleProcessorConfig reportConfig;

        public IntraDayReportScheduleProcessor(ILogger<IntraDayReportScheduler> logger,
                                                IQueueService<IntraDaySchedule> queueService,
                                                IPowerServiceClient powerServiceClient,
                                                IIntraDayReportCsvWriter intraDayReportCsvWriter,
                                                IntraDayReportScheduleProcessorConfig reportConfig)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.powerServiceClient = powerServiceClient;
            this.intraDayReportCsvWriter = intraDayReportCsvWriter;
            this.reportConfig = reportConfig;
        }

        public async Task Start(CancellationToken token)
        {
            logger.LogInformation("Starting schdule processor");

            while (!token.IsCancellationRequested)
            {
                await Actions.ExecuteWithErrorHandle(async () =>
                {
                    var schedule = await queueService.ReadAsync();
                    if (schedule != null)
                        await ProcessSchedule(schedule);
                    else
                        await Task.Delay(DelayTimeWhenScheduleIsNotFound * 1000);
                }, logger);
            }
            logger.LogInformation("Stopping schdule processor");
        }

        private async Task ProcessSchedule(IntraDaySchedule schedule)
        {
            var nextDayDate = schedule.ScheduleLocalTime.ToNextDayDate();
            var trades = await powerServiceClient.GetTradesAsync(nextDayDate);

            var aggregatedTrades = trades.ToAggregated(schedule.ScheduleLocalTime, reportConfig.LocalTimezoneId);

            await intraDayReportCsvWriter.GenerateAsync(schedule.ScheduleUtcTime, nextDayDate, aggregatedTrades);
        }
    }
}
