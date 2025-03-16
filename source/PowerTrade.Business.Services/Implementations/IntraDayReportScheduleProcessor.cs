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
        private readonly ILogger<IntraDayReportScheduleProcessor> logger;
        private readonly IQueueService<IntraDaySchedule> queueService;
        private readonly IPowerServiceClient powerServiceClient;
        private readonly IIntraDayReportCsvWriter intraDayReportCsvWriter;
        private readonly IntraDayReportScheduleProcessorConfig reportConfig;

        public IntraDayReportScheduleProcessor(ILogger<IntraDayReportScheduleProcessor> logger,
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
            logger.LogInformation("Starting schedule processor");

            while (!token.IsCancellationRequested)
            {
                await Actions.ExecuteWithErrorHandle(async () =>
                {
                    var schedule = await queueService.ReadAsync();
                    if (schedule != null)
                        await ProcessScheduleWithRetry(schedule);
                    else
                    {
                        logger.LogInformation("No schedule to process");
                        await Task.Delay(DelayTimeWhenScheduleIsNotFound * 1000);
                    }

                }, logger);
            }
            logger.LogInformation("Stopping schedule processor");
        }

        private async Task ProcessScheduleWithRetry(IntraDaySchedule schedule)
        {
            try
            {
                // Immdiate retries
                await Actions.ExecuteWithRetry<Exception, IntraDayReportScheduleProcessor>(async () =>
                {
                    await ProcessSchedule(schedule);
                },
                logger);
            }
            catch (Exception ex)
            {
                schedule.IncrementRetry();
                if (schedule.RetryCount <= 3)
                {
                    // [TODO]: Imporve Design:
                    // I would have preferred something here that supports delayed retry but current choice of 
                    // IQueue implementation doesnot support this.
                    await queueService.AddAsync(schedule);
                }
                else
                    logger.LogError("Schedule at local time {@scheduleTime} cannot be processed after retries", ex, schedule.ScheduleLocalTime);
            }
        }

        private async Task ProcessSchedule(IntraDaySchedule schedule)
        {
            var nextDayDate = schedule.ScheduleLocalTime.ToNextDayDate();
            logger.LogDebug("Requesting trade data for {@nextDayDate}", nextDayDate);

            var trades = await powerServiceClient.GetTradesAsync(nextDayDate);
            logger.LogDebug("Response from PowerService: {@trades}", (object)trades);

            var aggregatedTrades = trades.ToAggregated(schedule.ScheduleLocalTime, reportConfig.LocalTimezoneId);
            logger.LogDebug("Aggregated Data: {@aggregatedTrades}", (object)aggregatedTrades);

            await intraDayReportCsvWriter.GenerateAsync(schedule.ScheduleUtcTime, nextDayDate, aggregatedTrades);
        }
    }
}
