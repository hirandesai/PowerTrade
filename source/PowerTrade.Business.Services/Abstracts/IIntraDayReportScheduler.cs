namespace PowerTrade.Business.Services.Abstracts
{
    public interface IIntraDayReportScheduler
    {
        Task Start(CancellationToken token);
    }
}
