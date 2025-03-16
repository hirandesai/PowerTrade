using PowerTrade.Services.Dto;

namespace PowerTrade.Services.Abstracts
{
    public interface IPowerServiceClient
    {
        Task<PowerTradeRecord[]> GetTradesAsync(DateTime time);
    }
}
