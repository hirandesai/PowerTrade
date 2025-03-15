using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#if DEBUG
Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#endif

var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
var builder = Host.CreateDefaultBuilder(args)                    
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
                        services.AddLogging();
                    })
                    .UseConsoleLifetime()
                    .Build();

var logger = builder.Services.GetService<ILogger<Program>>();
logger.LogInformation($"{nameof(PowerTrade)} starting");

await builder.RunAsync();

