using Microsoft.Extensions.Logging;

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
    }
}
