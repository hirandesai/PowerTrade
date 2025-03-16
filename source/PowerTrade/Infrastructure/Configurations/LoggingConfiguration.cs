using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PowerTrade.Infrastructure.Configurations
{
    public static class LoggingConfiguration
    {
        public static void AddAppLogging(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)                            
                            .CreateLogger();

            serviceProvider.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog(Log.Logger);
            });
        }
    }
}
