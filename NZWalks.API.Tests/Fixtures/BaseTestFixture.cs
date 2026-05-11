using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Configuration;
using NUnit.Framework;

namespace NZWalks.API.Tests.Fixtures
{
    [SetUpFixture]
    public class BaseTestFixture
    {
        protected RegionsApiClient ApiClient { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            ApiClient = new RegionsApiClient(TestConfiguration.ApiBaseUrl);
        }

        [TearDown]
        public virtual void TearDown()
        {
            // Cleanup after each test if needed
        }
    }
}
