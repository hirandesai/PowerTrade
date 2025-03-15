using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Abstracts;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduler
    {
        private readonly IntraDayReportSchedulerConfig config;
        private readonly IQueueService<IntraDaySchedule> queueService;
        private readonly IDateTimeProvieder dateTimeProvieder;

        public IntraDayReportScheduler(IntraDayReportSchedulerConfig config,
                                        IQueueService<IntraDaySchedule> queueService,
                                        IDateTimeProvieder dateTimeProvieder)
        {
            this.config = config;
            this.queueService = queueService;
            this.dateTimeProvieder = dateTimeProvieder;
        }

        public async Task Start(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                await queueService.AddAsync(new IntraDaySchedule(dateTimeProvieder.CurrentTime));

                await Task.Delay(GetWaitTimeInMs);
            }
        }

        private int GetWaitTimeInMs => config.FrequencyMin * 60 * 1000;
    }
}
