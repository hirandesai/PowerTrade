using Axpo;
using PowerTrade.Services.Abstracts;
using PowerTrade.Services.Dto;

namespace PowerTrade.Services.Implementations
{
    public class PowerServiceClient : IPowerServiceClient
    {
        private readonly IPowerService powerService;

        public PowerServiceClient(IPowerService powerService)
        {
            this.powerService = powerService;
        }

        public async Task<PowerTradeRecord[]> GetTradesAsync(DateTime time)
        {
            var powerTrades = await powerService.GetTradesAsync(time);
            return powerTrades.Select(s => (PowerTradeRecord)s).ToArray();
        }
    }
}
