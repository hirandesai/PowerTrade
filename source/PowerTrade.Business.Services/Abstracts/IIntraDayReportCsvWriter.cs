using PowerTrade.Business.Services.Dtos;

namespace PowerTrade.Business.Services.Abstracts
{
    public interface IIntraDayReportCsvWriter
    {
        Task GenerateAsync(DateTime scheduleUtcTime, DateTime tradeDate, AggregatedPowerTradeRecord[] records);
    }
}
