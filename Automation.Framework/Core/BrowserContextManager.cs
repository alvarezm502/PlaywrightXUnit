using Automation.Framework.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Automation.Framework.Core
{
    /// <summary>
    /// Responsible for creating properly configured Playwright BrowserContext
    /// and Page instances for tests.
    ///
    /// This factory exists to centralize browser context creation logic so that
    /// all tests run with consistent settings and behavior.
    ///
    /// Responsibilities:
    /// - Retrieve the browser instance from PlaywrightManager
    /// - Create isolated BrowserContext instances for tests
    /// - Configure context settings such as timeouts
    /// - Create new Page instances for test execution
    /// 
    /// </summary>
    public class BrowserContextManager
    {
        private readonly PlaywrightManager _playwrightManager;
        private readonly TestSettings _settings;
        public BrowserContextManager(PlaywrightManager playwrightManager, TestSettings settings)
        {
            /// <summary>
            /// Constructor receives dependencies through Dependency Injection
            /// </summary>
            _playwrightManager = playwrightManager;
            _settings = settings;
         //   _logger = loggerManager.CreateLogger<BrowserContextManager>();
        }
        public async Task<IBrowserContext> CreateContextAsync()
        {
            /// <summary>
            /// Creates a new BrowserContext with settings from TestSettings.
            /// This ensures all tests run with consistent context configuration.
            /// </summary>
         //   _logger.LogInformation("Creating new browser context.");
            var browser = await _playwrightManager.GetBrowserAsync();
            var contextOptions = new BrowserNewContextOptions
            {
                BaseURL = _settings.BaseUrl,
                ViewportSize = new ViewportSize { Width = 1280, Height = 720 },
                IgnoreHTTPSErrors = true,
                AcceptDownloads = true,
                // Additional context options can be set here based on TestSettings
            };
            var context = await browser.NewContextAsync(contextOptions);
            context.SetDefaultTimeout(_settings.Timeout); // Convert seconds to milliseconds
          //  _logger.LogInformation("Created new BrowserContext with BaseURL: {BaseUrl} and Timeout: {Timeout}s", _settings.BaseUrl, _settings.Timeout);
            return context;
        }

        public async Task<IPage> CreatePageAsync()
        {
            /// <summary>
            /// Creates a new Page instance from a fresh BrowserContext.
            /// This is the main method that tests will call to get a ready-to-use Page for test execution.
            /// </summary>
          //  _logger.LogInformation("Creating new browser page.");
            var context = await CreateContextAsync();
            var page = await context.NewPageAsync();
            return page;
        }
    }
}
