using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Fixtures;
using NUnit.Framework;

namespace NZWalks.API.Tests.Tests
{
    [TestFixture]
    public class GetRegionByIdTests : BaseTestFixture
    {
        [Test]
        [Description("Verify that GetById API returns a successful response for a valid region ID")]
        public async Task GetRegionById_WithValidId_ShouldReturnOkStatus()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(allRegionsResponse.Data, Is.Not.Empty, "No regions available for testing");
            var validId = allRegionsResponse.Data.First().Id;

            // Act
            var response = await ApiClient.GetRegionByIdAsync(validId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), 
                "API should return 200 OK status code for valid ID");
            Assert.That(response.IsSuccessful, Is.True, 
                "Response should be successful");
        }

        [Test]
        [Description("Verify that GetById API returns correct region data")]
        public async Task GetRegionById_WithValidId_ShouldReturnCorrectRegionData()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(allRegionsResponse.Data, Is.Not.Empty, "No regions available for testing");
            var expectedRegion = allRegionsResponse.Data.First();

            // Act
            var response = await ApiClient.GetRegionByIdAsync(expectedRegion.Id);

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(expectedRegion.Id), 
                "Returned region ID should match the requested ID");
            Assert.That(response.Data.Code, Is.EqualTo(expectedRegion.Code), 
                "Returned region code should match");
            Assert.That(response.Data.Name, Is.EqualTo(expectedRegion.Name), 
                "Returned region name should match");
        }

        [Test]
        [Description("Verify that GetById API returns 404 Not Found for non-existent region")]
        public async Task GetRegionById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await ApiClient.GetRegionByIdAsync(invalidId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound), 
                "API should return 404 Not Found for non-existent region");
            Assert.That(response.IsSuccessful, Is.False, 
                "Response should not be successful");
        }

        [Test]
        [Description("Verify that GetById API returns region with all required properties")]
        public async Task GetRegionById_ShouldReturnRegionWithAllRequiredProperties()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(allRegionsResponse.Data, Is.Not.Empty, "No regions available for testing");
            var regionId = allRegionsResponse.Data.First().Id;

            // Act
            var response = await ApiClient.GetRegionByIdAsync(regionId);

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.Not.EqualTo(Guid.Empty), 
                "Region ID should not be empty");
            Assert.That(response.Data.Code, Is.Not.Null.And.Not.Empty, 
                "Region Code should not be null or empty");
            Assert.That(response.Data.Name, Is.Not.Null.And.Not.Empty, 
                "Region Name should not be null or empty");
        }

        [Test]
        [Description("Verify that GetById API response time is within acceptable limits")]
        public async Task GetRegionById_ResponseTimeShouldBeAcceptable()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(allRegionsResponse.Data, Is.Not.Empty, "No regions available for testing");
            var regionId = allRegionsResponse.Data.First().Id;

            // Act
            var startTime = DateTime.UtcNow;
            var response = await ApiClient.GetRegionByIdAsync(regionId);
            var endTime = DateTime.UtcNow;
            var responseTime = endTime - startTime;

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(responseTime.TotalSeconds, Is.LessThan(TestConfiguration.TimeoutSeconds), 
                $"Response time should be less than {TestConfiguration.TimeoutSeconds} seconds");
        }

        [Test]
        [Description("Verify that GetById API returns consistent data across multiple calls")]
        public async Task GetRegionById_ShouldReturnConsistentData()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(allRegionsResponse.Data, Is.Not.Empty, "No regions available for testing");
            var regionId = allRegionsResponse.Data.First().Id;

            // Act
            var response1 = await ApiClient.GetRegionByIdAsync(regionId);
            var response2 = await ApiClient.GetRegionByIdAsync(regionId);

            // Assert
            Assert.That(response1.IsSuccessful, Is.True);
            Assert.That(response2.IsSuccessful, Is.True);
            Assert.That(response1.Data.Id, Is.EqualTo(response2.Data.Id));
            Assert.That(response1.Data.Code, Is.EqualTo(response2.Data.Code));
            Assert.That(response1.Data.Name, Is.EqualTo(response2.Data.Name));
        }
    }
}
