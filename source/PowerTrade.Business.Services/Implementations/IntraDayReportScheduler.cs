using Microsoft.Extensions.Logging;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduler: IIntraDayReportScheduler
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
                var currentTime = dateTimeProvieder.CurrentTime;
                logger.LogInformation($"Triggering new scheule at {currentTime}");
                
                await queueService.AddAsync(new IntraDaySchedule(currentTime));
                
                await Task.Delay(GetWaitTimeInMs);
            }
            logger.LogInformation("Stopping scheduler, cancellation was requested");
        }

        private int GetWaitTimeInMs => config.FrequencyInSec * 1000;
    }
}
