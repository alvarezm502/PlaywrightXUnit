using Microsoft.Playwright;
using Automation.Framework.Models;

namespace Automation.Framework.Core
{
    /// <summary>
    /// Responsible for managing the lifecycle of Playwright and the browser instance.
    ///
    /// This class acts as the central entry point for any component that needs access
    /// to a browser. It is designed to be registered with Dependency Injection so that
    /// test projects can request it when needed.
    ///
    /// Responsibilities:
    /// - Initialize Playwright
    /// - Launch the configured browser (Chromium, Firefox, WebKit)
    /// - Provide access to the browser instance
    /// - Properly dispose Playwright resources when the test run ends
    /// </summary>
    public class PlaywrightManager : IAsyncDisposable
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private readonly TestSettings _settings;
        
        public PlaywrightManager(TestSettings settings)
        {
            /// <summary>
            /// Constructor receives framework settings through Dependency Injection
            /// </summary>
            _settings = settings;
        }

        public async Task<IBrowser> GetBrowserAsync()
        {
            /// <summary>
            /// Returns the active browser instance
            /// If the browser has not yet been created, this method will initialized 
            /// Playwright and launch the configured browser automatically
            /// </summary>
            if (_browser != null)
                return _browser;

            await InitializeAsync();

            return _browser;
        }

        public IBrowser Browser => _browser;
        public async Task InitializeAsync()
        {
            /// <summary>
            /// Initilizes Playwright and launches the configured browser.
            /// </summary>

            _playwright = await Playwright.CreateAsync();

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            };

            _browser = _settings.DriverType switch
            {
                DriverType.Firefox => await _playwright.Firefox.LaunchAsync(launchOptions),
                DriverType.Webkit => await _playwright.Webkit.LaunchAsync(launchOptions),
                DriverType.Edge => await _playwright.Chromium.LaunchAsync(launchOptions),
                DriverType.Chromium => await _playwright.Chromium.LaunchAsync(launchOptions),
                _ => await _playwright.Chromium.LaunchAsync(launchOptions)
            };
        }

        public async ValueTask DisposeAsync()
        {
            ///<summary>
            ///Properly dispose browser and Playwright resources.
            /// </summary>

            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
