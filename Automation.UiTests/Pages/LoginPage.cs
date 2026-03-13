using Automation.Framework.Core;
using Microsoft.Playwright;

namespace Automation.UiTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly LoggerManager _logger;

        public LoginPage(IPage page, LoggerManager logger)
        {
            _page = page;
            _logger = logger;
        }

        private ILocator Username => _page.Locator("#username");
        private ILocator Password => _page.Locator("#password");
        private ILocator LoginButton => _page.Locator("button[type='submit']");
        private ILocator FlashMessage => _page.Locator("#flash");

        /// <summary>
        /// Performs login with provided credentials
        /// </summary>
        public async Task LoginAsync(string username, string password)
        {
            _logger.LogInfo("Valid login");
            await Username.FillAsync(username);
            await Password.FillAsync(password);

            await LoginButton.ClickAsync();
        }

        /// <summary>
        /// Returns the flash message text after login
        /// </summary>
        public async Task<string> GetMessageAsync()
        {
            return await FlashMessage.InnerTextAsync();
        }
    }
}
