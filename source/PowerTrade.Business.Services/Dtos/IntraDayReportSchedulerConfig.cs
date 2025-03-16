namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDayReportSchedulerConfig
    {
        public int FrequencyInSec { get; private set; }
        
        public IntraDayReportSchedulerConfig(int frequencyMin)
        {
            FrequencyInSec = frequencyMin;
        }
    }
}
