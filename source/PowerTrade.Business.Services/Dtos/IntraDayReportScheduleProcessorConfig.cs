namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDayReportScheduleProcessorConfig
    {
        public string LocalTimezoneId { get; private set; }
        
        public IntraDayReportScheduleProcessorConfig(string localTimezoneId)
        {
            LocalTimezoneId = localTimezoneId;
        }
    }
}
