using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Fixtures;
using NUnit.Framework;

namespace NZWalks.API.Tests.Tests
{
    [TestFixture]
    public class GetAllRegionsTests : BaseTestFixture
    {
        [Test]
        [Description("Verify that GetAll API returns a successful response with status code 200")]
        public async Task GetAllRegions_ShouldReturnOkStatus()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), 
                "API should return 200 OK status code");
            Assert.That(response.IsSuccessful, Is.True, 
                "Response should be successful");
        }

        [Test]
        [Description("Verify that GetAll API returns a list of regions")]
        public async Task GetAllRegions_ShouldReturnListOfRegions()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True, 
                "Response should be successful");
            Assert.That(response.Data, Is.Not.Null, 
                "Response data should not be null");
            Assert.That(response.Data, Is.InstanceOf<List<RegionDto>>(), 
                "Response data should be a list");
        }

        [Test]
        [Description("Verify that each region in the response has required properties")]
        public async Task GetAllRegions_RegionsShouldHaveRequiredProperties()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True, 
                "Response should be successful");
            Assert.That(response.Data.Count, Is.GreaterThan(0), 
                "Should return at least one region");

            foreach (var region in response.Data)
            {
                Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty), 
                    "Region ID should not be empty");
                Assert.That(region.Code, Is.Not.Null.And.Not.Empty, 
                    "Region Code should not be null or empty");
                Assert.That(region.Name, Is.Not.Null.And.Not.Empty, 
                    "Region Name should not be null or empty");
            }
        }

        [Test]
        [Description("Verify that GetAll API returns valid Region DTOs with all properties")]
        public async Task GetAllRegions_ShouldReturnValidRegionDtos()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data, Is.Not.Empty);

            var region = response.Data.First();
            Assert.That(region.Id, Is.TypeOf<Guid>());
            Assert.That(region.Code, Is.TypeOf<string>());
            Assert.That(region.Name, Is.TypeOf<string>());
            Assert.That(region.RegionImageUrl, Is.TypeOf<string>().Or.Null);
        }

        [Test]
        [Description("Verify that GetAll API response time is within acceptable limits")]
        public async Task GetAllRegions_ResponseTimeShouldBeAcceptable()
        {
            // Act
            var startTime = DateTime.UtcNow;
            var response = await ApiClient.GetAllRegionsAsync();
            var endTime = DateTime.UtcNow;
            var responseTime = endTime - startTime;

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(responseTime.TotalSeconds, Is.LessThan(TestConfiguration.TimeoutSeconds), 
                $"Response time should be less than {TestConfiguration.TimeoutSeconds} seconds");
        }
    }
}
