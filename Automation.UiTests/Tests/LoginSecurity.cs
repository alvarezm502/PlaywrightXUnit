using Automation.Framework.Core;
using Automation.UiTests.Fixtures;
using Automation.UiTests.Pages;
using Automation.UiTests.TestData;

namespace Automation.UiTests.Tests
{
    public class LoginSecurityTests : BaseTest
    {
        public LoginSecurityTests(TestFixture fixture) : base(fixture) { }

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
                Logger.LogInfo("Attempting login with correct credentials");
                var user = _userSecrets.GetUser("Admin");
                var page = new LoginPage(Page, Logger);

                await page.LoginAsync(user.Username, user.Password);

                var message = await page.GetMessageAsync();

                Assert.Contains("You logged into a secure area!", message);
            });
        }
    }
}
