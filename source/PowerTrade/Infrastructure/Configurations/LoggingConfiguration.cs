using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PowerTrade.Infrastructure.Configurations
{
    public static class LoggingConfiguration
    {
        public static void AddLogging(IServiceCollection serviceProvider)
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

            serviceProvider.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });
        }
    }
}
