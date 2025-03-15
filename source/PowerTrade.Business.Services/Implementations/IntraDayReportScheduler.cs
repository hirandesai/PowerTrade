using PowerTrade.Business.Services.Dtos;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportScheduler
    {
        private readonly IntraDayReportSchedulerConfig config;

        public IntraDayReportScheduler(IntraDayReportSchedulerConfig config)
        {
            this.config = config;
        }

        public async Task Start(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                await Task.Delay(GetWaitTimeInMs);
            }
        }

        private int GetWaitTimeInMs => config.FrequencyMin * 60 * 1000;
    }
}
