using Microsoft.Extensions.Logging;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Business.Services.Extensions;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduler : IIntraDayReportScheduler
    {
        private readonly ILogger<IntraDayReportScheduler> logger;
        private readonly IQueueService<IntraDaySchedule> queueService;
        private readonly IDateTimeProvieder dateTimeProvieder;
        private readonly IntraDayReportSchedulerConfig config;

        public IntraDayReportScheduler(ILogger<IntraDayReportScheduler> logger,
                                        IQueueService<IntraDaySchedule> queueService,
                                        IDateTimeProvieder dateTimeProvieder,
                                        IntraDayReportSchedulerConfig config)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.dateTimeProvieder = dateTimeProvieder;
            this.config = config;
        }

        public async Task Start(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Actions.ExecuteWithErrorHandle(async () =>
                {
                    var currentLocalTime = dateTimeProvieder.CurrentTime;
                    var currentUtcTime = dateTimeProvieder.CurrentUtcTime;
                    logger.LogInformation($"Triggering new scheule at local {currentLocalTime}");

                    await queueService.AddAsync(new IntraDaySchedule(currentLocalTime, currentUtcTime));

                    await Task.Delay(GetWaitTimeInMs);
                }, logger);

            }
            logger.LogInformation("Stopping scheduler, cancellation was requested");
        }

        private int GetWaitTimeInMs => config.FrequencyInSec * 1000;
    }
}
