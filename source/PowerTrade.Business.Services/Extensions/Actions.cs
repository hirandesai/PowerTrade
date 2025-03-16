using Microsoft.Extensions.Logging;
using Polly;

namespace PowerTrade.Business.Services.Extensions
{
    public static class Actions
    {
        public static async Task ExecuteWithErrorHandle<T>(Func<Task> action, ILogger<T> logger)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public static async Task ExecuteWithRetry<TException, TLogger>(Func<Task> task,
                                                        ILogger<TLogger> logger,
                                                        int retries = 3,
                                                        bool isDelayedRetry = false)
            where TException : Exception
        {
            var retry = Policy
                        .Handle<TException>()
                        .WaitAndRetryAsync(retries,
                                        retryAttempt => isDelayedRetry ? TimeSpan.FromSeconds(0.5 * retryAttempt) : TimeSpan.FromSeconds(0.5),
                                        onRetry: (exception, calculatedWaitDuration) =>
                                        {
                                            logger.LogWarning(exception, "[Retry]");
                                        });

            await retry.ExecuteAsync(async () =>
            {
                await task();
            });
        }
    }
}
