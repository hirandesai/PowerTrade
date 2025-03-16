using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PowerTrade.Infrastructure.Configurations;

#if DEBUG
Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#endif

var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
var builder = Host.CreateDefaultBuilder(args)
                    .ConfigureHostOptions(hostOptions =>
                    {
                        hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                    })
                    .ConfigureAppConfiguration(config =>
                    {
                        config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env}.json", true, true)
                        .AddEnvironmentVariables();

                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddAppLogging(hostContext.Configuration);
                        services.AddAppServices(hostContext.Configuration);
                    })
                    .UseConsoleLifetime()
                    .Build();

var logger = builder.Services.GetService<ILogger<Program>>();
logger.LogInformation($"{nameof(PowerTrade)} starting");

await builder.RunAsync();

