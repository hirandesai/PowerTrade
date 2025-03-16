namespace PowerTrade.Dto
{
    public class IntraDayReportConfig
    {
        public int SchedulerFrequencyInSec { get; set; }
        public string LocalTimeZoneId { get; set; }
        public string ReportPath { get; set; }
        public string CsvDelimiter { get; set; }
    }
}
