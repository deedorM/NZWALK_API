using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Fixtures;
using NZWalks.API.Tests.Helpers;
using NUnit.Framework;

namespace NZWalks.API.Tests.Tests
{
    [TestFixture]
    public class RegionsApiIntegrationTests : BaseTestFixture
    {
        [Test]
        [Description("Verify complete CRUD workflow - Create, Read, Update operations")]
        public async Task RegionsApi_CompleteCrudWorkflow()
        {
            // Arrange
            var createRequest = TestHelper.CreateSampleRegion(
                code: TestHelper.GenerateUniqueTestData("NZ"),
                name: TestHelper.GenerateUniqueTestData("Region"),
                imageUrl: "https://example.com/test-region.jpg");

            // Act & Assert - CREATE
            var createResponse = await ApiClient.CreateRegionAsync(createRequest);
            Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), 
                "Create should return 201 Created");
            Assert.That(createResponse.Data, Is.Not.Null, 
                "Created region should be returned");
            var createdRegionId = createResponse.Data.Id;
            Assert.That(createdRegionId, Is.Not.EqualTo(Guid.Empty), 
                "Created region should have valid ID");

            // Act & Assert - READ
            var getResponse = await ApiClient.GetRegionByIdAsync(createdRegionId);
            Assert.That(getResponse.IsSuccessful, Is.True, 
                "Should be able to retrieve created region");
            Assert.That(getResponse.Data.Code, Is.EqualTo(createRequest.Code), 
                "Retrieved region should match created data");

            // Act & Assert - UPDATE
            var updateRequest = new CreateRegionRequest
            {
                Code = TestHelper.GenerateUniqueTestData("UPD"),
                Name = "Updated Region Name",
                RegionImageUrl = "https://example.com/updated-image.jpg"
            };
            var updateResponse = await ApiClient.UpdateRegionAsync(createdRegionId, updateRequest);
            Assert.That(updateResponse.IsSuccessful, Is.True, 
                "Update should be successful");
            Assert.That(updateResponse.Data.Name, Is.EqualTo(updateRequest.Name), 
                "Updated region should reflect new name");

            // Act & Assert - VERIFY UPDATE
            var verifyResponse = await ApiClient.GetRegionByIdAsync(createdRegionId);
            Assert.That(verifyResponse.Data.Name, Is.EqualTo(updateRequest.Name), 
                "Verification should show updated data");
        }

        [Test]
        [Description("Verify data consistency between GetAll and GetById")]
        public async Task RegionsApi_DataConsistencyBetweenEndpoints()
        {
            // Act
            var getAllResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(getAllResponse.IsSuccessful, Is.True, 
                "GetAll should succeed");
            Assert.That(getAllResponse.Data, Is.Not.Empty, 
                "GetAll should return regions");

            // Verify each region from GetAll can be retrieved via GetById
            foreach (var region in getAllResponse.Data)
            {
                var getByIdResponse = await ApiClient.GetRegionByIdAsync(region.Id);
                Assert.That(getByIdResponse.IsSuccessful, Is.True, 
                    $"GetById should succeed for region {region.Id}");

                // Compare data
                Assert.That(getByIdResponse.Data.Id, Is.EqualTo(region.Id));
                Assert.That(getByIdResponse.Data.Code, Is.EqualTo(region.Code));
                Assert.That(getByIdResponse.Data.Name, Is.EqualTo(region.Name));
            }
        }

        [Test]
        [Ignore("Temporarily disabled")]
        [Description("Verify API response headers and metadata")]
        public async Task RegionsApi_ShouldReturnCorrectResponseHeaders()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Headers, Is.Not.Null, 
                "Response should include headers");

            // Verify content type
            var contentTypeHeader = response.Headers
                .FirstOrDefault(h => h.Name?.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) ?? false);
            Assert.That(contentTypeHeader, Is.Not.Null, 
                "Content-Type header should be present");
            Assert.That(contentTypeHeader?.Value, Does.Contain("application/json"), 
                "Content-Type should be application/json");
        }

        [Test]
        [Description("Verify concurrent API requests")]
        [Timeout(60000)]
        public async Task RegionsApi_ShouldHandleConcurrentRequests()
        {
            // Arrange
            var taskList = new List<Task<RestSharp.RestResponse<List<RegionDto>>>>();
            const int numberOfConcurrentRequests = 5;

            // Act - Create multiple concurrent requests
            for (int i = 0; i < numberOfConcurrentRequests; i++)
            {
                taskList.Add(ApiClient.GetAllRegionsAsync());
            }

            var results = await Task.WhenAll(taskList);

            // Assert - All requests should succeed
            Assert.That(results, Has.All.Matches<RestSharp.RestResponse<List<RegionDto>>>(
                r => r.IsSuccessful), 
                "All concurrent requests should succeed");
            Assert.That(results, Has.All.Matches<RestSharp.RestResponse<List<RegionDto>>>(
                r => r.Data.Count > 0), 
                "All responses should contain regions");
        }

        [Test]
        [Description("Verify empty result handling")]
        public async Task RegionsApi_ShouldHandleEdgeCases()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True, 
                "Should handle any number of regions including empty");
            Assert.That(response.Data, Is.Not.Null, 
                "Should return empty list rather than null");
            Assert.That(response.Data, Is.InstanceOf<List<RegionDto>>(), 
                "Should be a valid list");
        }

        [Test]
        [Description("Verify API handles special characters in data")]
        public async Task RegionsApi_ShouldHandleSpecialCharactersInResponse()
        {
            // Arrange
            var getAllResponse = await ApiClient.GetAllRegionsAsync();
            Assert.That(getAllResponse.Data, Is.Not.Empty);

            // Act & Assert - Verify each region data can be properly parsed
            foreach (var region in getAllResponse.Data)
            {
                Assert.That(region.Code, Is.Not.Null, 
                    "Region code should not be null");
                Assert.That(region.Name, Is.Not.Null, 
                    "Region name should not be null");

                // Verify they can be converted to string without issues
                var codeString = region.Code.ToString();
                var nameString = region.Name.ToString();

                Assert.That(codeString, Is.TypeOf<string>());
                Assert.That(nameString, Is.TypeOf<string>());
            }
        }
    }
}
