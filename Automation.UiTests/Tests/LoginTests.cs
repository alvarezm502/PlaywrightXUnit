using Automation.Framework.Core;
using Automation.UiTests.Fixtures;
using Automation.UiTests.Pages;
using Automation.UiTests.TestData;

namespace Automation.UiTests.Tests
{
    [CollectionDefinition("Playwright Collection")]
    public class LoginTests : BaseTest
    {
        public LoginTests(TestFixture fixture) : base(fixture) { }

        /// <summary>
        /// Valid login test (should pass)
        /// </summary>
        [Fact]
        public async Task Login001_WithValidCredentials_ShouldSucceed()
        {
            await RunAsync(async () =>
            {
                var loginPage = new LoginPage(Page, Logger);

                await loginPage.LoginAsync(Users.Admin, Mpw);

                var message = await loginPage.GetMessageAsync();

                Assert.Contains("You logged into a secure area!", message);
            });
        }

        /// <summary>
        /// Invalid login test (intentional failure example)
        /// </summary>
        [Fact]
        public async Task Login002_WithInvalidCredentials_ShouldFail()
        {
            await RunAsync(async () =>
            {
                Logger.LogInfo("I wonder if this works - this is LoginTests");
                var loginPage = new LoginPage(Page, Logger);

                await loginPage.LoginAsync(Users.TestUser1, "badpassword");

                var message = await loginPage.GetMessageAsync();

                // intentionally wrong assertion to trigger screenshot/log/html
                Assert.Contains("ThisWillFail", message);
            });
        }
    }
}
