using Microsoft.Extensions.DependencyInjection;
using Automation.Framework.Core;
using Microsoft.Playwright;

namespace Automation.UiTests.Fixtures
{
    /// <summary>
    /// TestFixture runs once per test class.
    /// 
    /// Responsibilities:
    /// - Initialize Dependency Injection
    /// - Initialize Playwright and Browser 
    /// - Dispose browser when test class finishes
    /// </summary>
    public class TestFixture : IAsyncLifetime
    {
        private ServiceProvider? _serviceProvider;
        private PlaywrightManager? _playwrightManager;

        public IServiceProvider ServiceProvider => _serviceProvider ?? throw new InvalidOperationException("Service provider has not been initialized.");
        public IBrowser Browser => _playwrightManager?.Browser ?? throw new InvalidOperationException("Browser manager has not been initialized.");

        public async ValueTask InitializeAsync()
        {
            // Build dependency injection container
            _serviceProvider = ServiceConfigurator.Configure<TestFixture>();

            //Resolve required framework services
            _playwrightManager = _serviceProvider.GetRequiredService<PlaywrightManager>();

            //Initialize Playwright and create browser instance
            await _playwrightManager.InitializeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            ///<summary>
            ///Called once when the fixture is disposed (end of collection).
            ///Properly dispose Browser and DI container
            /// </summary>
            //Dispose dependency injection container
            if (_serviceProvider != null)
                await _serviceProvider.DisposeAsync();
        }
    }
}
