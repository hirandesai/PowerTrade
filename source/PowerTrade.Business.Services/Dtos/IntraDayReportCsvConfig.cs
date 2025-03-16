namespace PowerTrade.Business.Services.Dtos
{
    public record IntraDayReportCsvConfig
    {
        public string FilePath { get; set; }
        public string CsvDelimiter { get; set; }

        public IntraDayReportCsvConfig(string filePath, string csvDelimiter)
        {
            FilePath = filePath;
            CsvDelimiter = csvDelimiter;
        }
    }
}
