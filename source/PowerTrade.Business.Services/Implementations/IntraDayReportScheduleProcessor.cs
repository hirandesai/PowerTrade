using Microsoft.Extensions.Logging;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduleProcessor : IIntraDayReportScheduleProcessor
    {
        private readonly ILogger<IntraDayReportScheduler> logger;
        private readonly IQueueService<IntraDaySchedule> queueService;
        private readonly IPowerServiceClient powerServiceClient;
        public IntraDayReportScheduleProcessor(ILogger<IntraDayReportScheduler> logger,
                                                IQueueService<IntraDaySchedule> queueService,
                                                IPowerServiceClient powerServiceClient)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.powerServiceClient = powerServiceClient;
        }

        public async Task Start(CancellationToken token)
        {
            logger.LogInformation("Starting schdule processor");

            while (!token.IsCancellationRequested)
            {
                var schedule = await queueService.ReadAsync();
                if (schedule != null)
                {
                    await ProcessSchedule(schedule);
                }
                await Task.Delay(1);
            }
            logger.LogInformation("Stopping schdule processor");
        }

        private async Task ProcessSchedule(IntraDaySchedule schedule)
        {
            var nextDayDate = schedule.ScheduleTime.Date.AddDays(1).AddMinutes(-1).Date;
            var trades = await powerServiceClient.GetTradesAsync(nextDayDate);
            
        }
    }
}
