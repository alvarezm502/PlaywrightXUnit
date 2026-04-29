using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Automation.Framework.Models;
using Serilog;

namespace Automation.Framework.Core
{
    /// <summary>
    /// Responsible for configuring and building the Dependency Injection container
    /// used by the automation framework.
    ///
    /// This class acts as the bootstrapper for the entire framework. It wires together
    /// configuration, logging, and core services such as the PlaywrightManager.
    ///
    /// The goal is to keep all framework initialization in one central location so
    /// test projects only need to call a single method to start using the framework.
    /// </summary>
    public static class ServiceConfigurator
    {
        public static ServiceProvider Configure<T>()
        {
            ///<summary>
            /// Configures the Dependency Injection container with all necessary services.
            /// </summary>
            /// 

            //Loading configuration from the test project's appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets(typeof(T).Assembly, optional: true)
                .AddEnvironmentVariables()
                .Build();

            //Injecting TestSettings object into framework services
            var settings = configuration
                .GetSection("TestSettings")
                .Get<TestSettings>()
                ?? throw new InvalidOperationException("Missing or invalid TestSettings configuration.");

            //Configure logging (Serilog)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Creating service collection and registering services
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<TestSettings>(settings);
            services.AddSingleton<PlaywrightManager>();
            services.AddSingleton<LoggerManager>();
            services.AddSingleton<BrowserContextManager>();
            services.AddSingleton<ScreenshotService>();
            services.AddSingleton<UserSecretsService>();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            //Build and return the service provider
            return services.BuildServiceProvider();
        }
    }
}
