namespace PowerTrade.Business.Services.Abstracts
{
    public interface IIntraDayReportScheduleProcessor
    {
        Task Start(CancellationToken token);
    }
}
