using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Automation.Framework.Core
{
    /// <summary>
    /// Provides functionality for capturing screenshots during test execution.
    ///
    /// This service is primarily used when a test fails so that the current
    /// browser state can be recorded for debugging purposes. Screenshots are
    /// saved to a configurable output directory where they can later be
    /// attached to HTML reports or test result artifacts.
    /// </summary>
    public class ScreenshotService
    {
        //private readonly ILogger<ScreenshotService> _logger;

        //public ScreenshotService(LoggerManager loggerManager)
        //{
        //    _logger = loggerManager.CreateLogger<ScreenshotService>();
        //}

        public async Task<string> CaptureAsync(IPage page, string testName)
        {
            /// <summary> 
            /// Captures a screenshot of the current state of the given Playwright page and saves it to disk.
            /// </summary>

            //Ensure screenshot directory exists
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDir);
            string pathToUse = directoryInfo.Parent?.Parent?.Parent?.FullName ?? currentDir;
            var screenshotsDir = Path.Combine(pathToUse, "TestResults", "Screenshots");
            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);

            //Generate a unique filename for the screenshot using the test name and timestamp
            var fileName = $"{testName}.png";
            var filePath = Path.Combine(screenshotsDir, fileName);
            //_logger.LogInformation("Capturing screenshot for test {TestName}", testName);

            //Capture full page screenshot
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = filePath,
                FullPage = true
            });

            return filePath;
        }
    }
}
