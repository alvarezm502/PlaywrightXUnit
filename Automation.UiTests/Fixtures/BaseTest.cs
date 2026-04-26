using Automation.UiTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Automation.Framework.Models;
using Microsoft.Playwright;
using System.Text;

namespace Automation.Framework.Core
{
    /// <summary>
    /// BaseTest handles per-test setup and teardown.
    /// 
    /// Responsibilities:
    /// - Create a new browser context and page per test (parallel-safe)
    /// - Automatically navigate to BaseUrl from settings
    /// - Capture exceptions during test execution
    /// - Take screenshot, write log, and generate HTML report if test fails
    /// </summary>
    public abstract class BaseTest : IClassFixture<TestFixture>, IAsyncLifetime
    {
        private readonly TestFixture _fixture;
        //private static readonly Dictionary<string, string> _users;
        private ScreenshotService _screenshotService = null!;
        private LoggerManager _loggerManager = null!;
        private TestSettings _settings = null!;
        private Exception? _exception;
        private string _testName = string.Empty;

        protected IBrowserContext Context = null!;
        protected IPage Page = null!;
        protected LoggerManager Logger = null!;
        protected UserSecretsService _userSecrets = null!;
       // protected Users Users;
        //  protected Dictionary<string, string> Users;
        protected BaseTest(TestFixture fixture)
        {
            _fixture = fixture;
        }
        
        public async ValueTask InitializeAsync()
        {
            /// <summary>
            /// Runs before each test.
            /// Sets up browser context, page, and resolves framework services.
            /// Automatically navigates to BaseUrl.
            /// </summary>
            var services = _fixture.ServiceProvider;

            _screenshotService = services.GetRequiredService<ScreenshotService>();
            _loggerManager = services.GetRequiredService<LoggerManager>();
            _settings = services.GetRequiredService<TestSettings>();
            _userSecrets = services.GetRequiredService<UserSecretsService>();
            Logger = services.GetRequiredService<LoggerManager>();

            //Create isolated browser context for this test
            Context = await _fixture.Browser.NewContextAsync();

            //Create page
            Page = await Context.NewPageAsync();

            //Nav to BaseUrl from settings
            await Page.GotoAsync(_settings.BaseUrl);
        }

        protected async Task RunAsync(Func<Task> test, [System.Runtime.CompilerServices.CallerMemberName] string testMethod = "")
        {
            /// <summary>
            /// Helper method to wrap test execution in try/catch
            /// </summary>
            try
            {
                var className = GetType().Name;

                _testName = $"{className}_{testMethod}";
                Logger.CreateTestLogger(_testName);
                await test();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            /// <summary>
            /// Runs after each test.
            /// 
            /// If test failed:
            /// - Captures screenshot
            /// - Writes log file
            /// - Generates simple HTML report referencing screenshot
            /// Always closes the browser context.
            /// </summary>
            if (_exception != null)
            {
                //string currentDir = Directory.GetCurrentDirectory();
                //DirectoryInfo directoryInfo = new DirectoryInfo(currentDir);
                string pathToUse = await GetProjectDir();//directoryInfo.Parent?.Parent?.Parent?.FullName ?? currentDir;
                var reportDir = Path.Combine(pathToUse, "TestResults", "Reports");
                if (!Directory.Exists(reportDir))
                    Directory.CreateDirectory(reportDir);

                var reportPath = Path.Combine(reportDir, _testName+".html");

                //Creating artifacts if test failed
                await _screenshotService.CaptureAsync(Page, _testName);

                _loggerManager.WriteLogIfFailed(_testName, _exception);
                var html = new StringBuilder();
                html.AppendLine("<html><body>");
                html.AppendLine($"<h2>Test Failed: {_testName}</h2>");
                html.AppendLine($"<p><b>Time:</b> {DateTime.Now}</p>");
                html.AppendLine("<h3>Exception</h3>");
                html.AppendLine($"<pre>{_exception}</pre>");
                html.AppendLine("<h3>Screenshot</h3>");
                html.AppendLine($"<img src='../Screenshots/{_testName}.png' width='800'/>");
                html.AppendLine("</body></html>");
                await File.WriteAllTextAsync(reportPath, html.ToString());
            }

            if (Context != null)
                await Context.CloseAsync();
        }

        public async Task<string> GetProjectDir()
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDir);
            string pathToUse = directoryInfo.Parent?.Parent?.Parent?.FullName ?? currentDir;
            return pathToUse;
        }
    }
}
