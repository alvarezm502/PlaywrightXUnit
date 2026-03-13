using Xunit;
using Automation.UiTests.Fixtures;

namespace Automation.UiTests.Collections
{
    [CollectionDefinition("Playwright Collection")]
    public class PlaywrightCollection : ICollectionFixture<TestFixture>
    {
    }
}
