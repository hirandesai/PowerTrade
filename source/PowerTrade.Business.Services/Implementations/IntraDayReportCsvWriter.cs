using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using PowerTrade.Business.Services.Abstracts;
using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Abstracts;
using System.Globalization;

namespace PowerTrade.Business.Services.Implementations
{
    public class IntraDayReportCsvWriter : IIntraDayReportCsvWriter
    {
        private ILogger<IntraDayReportCsvWriter> logger;
        private readonly IFileService fileService;
        private readonly IntraDayReportCsvConfig config;

        public IntraDayReportCsvWriter(ILogger<IntraDayReportCsvWriter> logger,
                                                IFileService fileService,
                                                IntraDayReportCsvConfig config)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.config = config;
        }

        public async Task GenerateAsync(DateTime scheduleUtcTime, DateTime tradeDate, AggregatedPowerTradeRecord[] records)
        {
            var fileName = $"PowerPosition_{tradeDate:yyyyMMdd}_{scheduleUtcTime:yyyyMMddHHmm}.csv";
            var filePath = Path.Join(config.FilePath, fileName);

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = config.CsvDelimiter,
                HasHeaderRecord = true
            };
            logger.LogInformation("Generating CSV file at {@filePath}", filePath);
            using (var streamWriter = fileService.CreateFileStream(filePath))
            using (var csv = new CsvWriter(streamWriter, csvConfig))
            {
                await csv.WriteRecordsAsync(records);
            }
            logger.LogInformation("CSV File generated at {@filePath}", filePath);
        }
    }
}
