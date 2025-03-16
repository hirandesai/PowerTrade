namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDayReportScheduleProcessorConfig
    {
        public string LocalTimeZoneId { get; private set; }
        
        public IntraDayReportScheduleProcessorConfig(string localTimeZoneId)
        {
            LocalTimeZoneId = localTimeZoneId;
        }
    }
}
