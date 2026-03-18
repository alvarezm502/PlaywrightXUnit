using Automation.Framework.Core;
using Automation.UiTests.Fixtures;
using Automation.UiTests.Pages;

namespace Automation.UiTests.Tests
{
    [CollectionDefinition("Playwright Collection")]
    public class SecuritiesPartTwo : BaseTest
    {
        public SecuritiesPartTwo(TestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldFail()
        {
            await RunAsync(async () =>
            {
                Logger.LogInfo("Attempting login with empty password");

                var page = new LoginPage(Page, Logger);
                var user = _userSecrets.GetUser("Admin");
                await page.LoginAsync(user.Username, "");

                var message = await page.GetMessageAsync();

                Assert.Contains("Your password is invalid!", message);
            });
        }

        [Fact]
        public async Task Login_WithCorrectPassword_ShouldPass()
        {
            await RunAsync(async () =>
            {
                Logger.LogInfo("Attempting login with correct credentials - this is Securiteis part two");

                var page = new LoginPage(Page, Logger);

                var user = _userSecrets.GetUser("Admin");
                await page.LoginAsync(user.Username, "SuperSecretPassword!123");

                var message = await page.GetMessageAsync();

                Assert.Contains("You logged into a secure area!", message);
            });
        }
    }
}
