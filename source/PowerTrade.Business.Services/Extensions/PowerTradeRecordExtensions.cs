using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PowerTrade.Business.Services.Extensions
{
    public static class PowerTradeRecordExtensions
    {
        public static AggregatedPowerTradeRecord[] ToAggregated(this PowerTradeRecord[] trades, DateTime tradeRequestTime)
        {
            TimeZoneInfo MadridTimezone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var tradeStartTime = new DateTime(tradeRequestTime.Year, tradeRequestTime.Month, tradeRequestTime.Day, tradeRequestTime.Hour, 00, 00);
            return trades
                    .SelectMany(q => q.Periods)
                    .Select(s =>

                    {
                        var tradeTime = tradeStartTime.AddHours(s.Period);
                        var utcDate = TimeZoneInfo.ConvertTimeToUtc(tradeTime, MadridTimezone);
                        return new
                        {
                            UtcDate = utcDate,
                            Volume = s.Volume
                        };
                    })
                    .GroupBy(g => g.UtcDate)
                    .Select(s => new AggregatedPowerTradeRecord(s.Key, s.Sum(g => g.Volume)))
                    .ToArray();
        }
    }
}
