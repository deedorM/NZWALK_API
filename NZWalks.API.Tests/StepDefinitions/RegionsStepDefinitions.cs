using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Configuration;
using Reqnroll;
using System.Diagnostics;
using NUnit.Framework;

namespace NZWalks.API.Tests.StepDefinitions
{
    [Binding]
    public class RegionsStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private RegionsApiClient _apiClient;
        private RestSharp.RestResponse<List<RegionDto>> _getAllResponse;
        private RestSharp.RestResponse<RegionDto> _getByIdResponse;
        private RestSharp.RestResponse _deleteResponse;
        private List<RegionDto> _allRegions;
        private Guid _currentRegionId;
        private DateTime _startTime;
        private DateTime _endTime;
        private CreateRegionRequest _currentRegionRequest;
        private List<Task<RestSharp.RestResponse<List<RegionDto>>>> _concurrentTasks;

        public RegionsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _apiClient = new RegionsApiClient(TestConfiguration.ApiBaseUrl);
            _concurrentTasks = new List<Task<RestSharp.RestResponse<List<RegionDto>>>>();
        }

        #region Given Steps

        [Given("I have obtained a valid region ID from the GetAll endpoint")]
        public async Task GivenIHaveObtainedValidRegionId()
        {
            _getAllResponse = await _apiClient.GetAllRegionsAsync();
            Assert.That(_getAllResponse.IsSuccessful, Is.True, "GetAll should succeed");
            Assert.That(_getAllResponse.Data, Is.Not.Empty, "Should have at least one region");
            _currentRegionId = _getAllResponse.Data.First().Id;
        }

        [Given("I have retrieved all regions")]
        public async Task GivenIHaveRetrievedAllRegions()
        {
            _getAllResponse = await _apiClient.GetAllRegionsAsync();
            Assert.That(_getAllResponse.IsSuccessful, Is.True, "GetAll should succeed");
            _allRegions = _getAllResponse.Data;
        }

        [Given("I have a new region to create with:")]
        public void GivenIHaveANewRegionToCreateWith(Table table)
        {
            var row = table.Rows.First();
            _currentRegionRequest = new CreateRegionRequest
            {
                Code = row["Code"],
                Name = row["Name"],
                RegionImageUrl = row["ImageUrl"]
            };
        }

        #endregion

        #region When Steps

        [When("I call the GetAll regions endpoint")]
        public async Task WhenICallGetAllRegionsEndpoint()
        {
            _startTime = DateTime.UtcNow;
            _getAllResponse = await _apiClient.GetAllRegionsAsync();
            _endTime = DateTime.UtcNow;
        }

        [When("I call the GetRegion by ID endpoint with that ID")]
        public async Task WhenICallGetRegionByIdEndpoint()
        {
            _startTime = DateTime.UtcNow;
            _getByIdResponse = await _apiClient.GetRegionByIdAsync(_currentRegionId);
            _endTime = DateTime.UtcNow;
        }

        [When("I call the GetRegion by ID endpoint with a non-existent region ID")]
        public async Task WhenICallGetRegionByIdWithNonExistentId()
        {
            var invalidId = Guid.NewGuid();
            _startTime = DateTime.UtcNow;
            _getByIdResponse = await _apiClient.GetRegionByIdAsync(invalidId);
            _endTime = DateTime.UtcNow;
        }

        [When("I retrieve each region individually by ID")]
        public async Task WhenIRetrieveEachRegionIndividuallyById()
        {
            foreach (var region in _allRegions)
            {
                var response = await _apiClient.GetRegionByIdAsync(region.Id);
                Assert.That(response.IsSuccessful, Is.True, $"GetById should succeed for region {region.Id}");
            }
        }

        [When("I create the region")]
        public async Task WhenICreateTheRegion()
        {
            _startTime = DateTime.UtcNow;
            var response = await _apiClient.CreateRegionAsync(_currentRegionRequest);
            _endTime = DateTime.UtcNow;
            _scenarioContext["createResponse"] = response;
            if (response.IsSuccessful && response.Data != null)
            {
                _currentRegionId = response.Data.Id;
            }
        }

        [When("I retrieve the created region by ID")]
        public async Task WhenIRetrieveTheCreatedRegionById()
        {
            _getByIdResponse = await _apiClient.GetRegionByIdAsync(_currentRegionId);
        }

        [When("I update the region with:")]
        public void WhenIUpdateTheRegionWith(Table table)
        {
            var row = table.Rows.First();
            _currentRegionRequest = new CreateRegionRequest
            {
                Code = row["Code"],
                Name = row["Name"],
                RegionImageUrl = row["ImageUrl"]
            };
        }

        [When("I update the region")]
        public async Task WhenIUpdateTheRegion()
        {
            var response = await _apiClient.UpdateRegionAsync(_currentRegionId, _currentRegionRequest);
            _scenarioContext["updateResponse"] = response;
        }

        [When("I delete the region")]
        public async Task WhenIDeleteTheRegion()
        {
            _deleteResponse = await _apiClient.DeleteRegionAsync(_currentRegionId);
        }

        [When("I make 5 concurrent requests to get all regions")]
        public async Task WhenIMakeConcurrentRequests()
        {
            _concurrentTasks.Clear();
            for (int i = 0; i < 5; i++)
            {
                _concurrentTasks.Add(_apiClient.GetAllRegionsAsync());
            }
            await Task.WhenAll(_concurrentTasks);
        }

        [When("I query each region individually")]
        public async Task WhenIQueryEachRegionIndividually()
        {
            foreach (var region in _allRegions)
            {
                var response = await _apiClient.GetRegionByIdAsync(region.Id);
                _scenarioContext["individualRegionResponse"] = response;
            }
        }

        #endregion

        #region Then Steps

        [Then("the response status code should be 200 OK")]
        public void ThenResponseStatusCodeShouldBe200Ok()
        {
            Assert.That(_getAllResponse?.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
                "API should return 200 OK");
        }

        [Then("the response status code should be 201 Created")]
        public void ThenResponseStatusCodeShouldBe201Created()
        {
            var response = _scenarioContext.Get<RestSharp.RestResponse<RegionDto>>("createResponse");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created),
                "API should return 201 Created");
        }

        [Then("the response status code should be 404 Not Found")]
        public void ThenResponseStatusCodeShouldBe404NotFound()
        {
            Assert.That(_getByIdResponse?.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound),
                "API should return 404 Not Found for invalid ID");
        }

        [Then("the response should contain a list of regions")]
        public void ThenResponseShouldContainListOfRegions()
        {
            Assert.That(_getAllResponse?.IsSuccessful, Is.True, "Response should be successful");
            Assert.That(_getAllResponse?.Data, Is.Not.Null, "Response data should not be null");
            Assert.That(_getAllResponse?.Data, Is.InstanceOf<List<RegionDto>>(), "Response should be a list");
        }

        [Then("each region in the response should have:")]
        public void ThenEachRegionShouldHaveProperties(Table table)
        {
            Assert.That(_getAllResponse?.Data, Is.Not.Empty, "Should have at least one region");

            var requiredProperties = table.Rows.Select(r => r["Property"]).ToList();
            foreach (var region in _getAllResponse.Data)
            {
                if (requiredProperties.Contains("Id"))
                    Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty), "Region ID should not be empty");
                if (requiredProperties.Contains("Code"))
                    Assert.That(region.Code, Is.Not.Null.And.Not.Empty, "Region Code should not be null or empty");
                if (requiredProperties.Contains("Name"))
                    Assert.That(region.Name, Is.Not.Null.And.Not.Empty, "Region Name should not be null or empty");
            }
        }

        [Then("the response should contain valid Region DTOs with all required properties")]
        public void ThenResponseShouldContainValidRegionDtos()
        {
            Assert.That(_getAllResponse?.IsSuccessful, Is.True);
            Assert.That(_getAllResponse?.Data, Is.Not.Empty);

            var region = _getAllResponse.Data.First();
            Assert.That(region.Id, Is.TypeOf<Guid>(), "Id should be Guid type");
            Assert.That(region.Code, Is.TypeOf<string>(), "Code should be string type");
            Assert.That(region.Name, Is.TypeOf<string>(), "Name should be string type");
        }

        [Then("the response time should be less than (.*) seconds")]
        public void ThenResponseTimeShouldBeLessThanSeconds(int seconds)
        {
            var responseTime = _endTime - _startTime;
            Assert.That(responseTime.TotalSeconds, Is.LessThan(seconds),
                $"Response time should be less than {seconds} seconds");
        }

        [Then("the response header Content-Type should contain \"application/json\"")]
        public void ThenResponseHeaderShouldContainJson()
        {
            Assert.That(_getAllResponse?.Headers, Is.Not.Null, "Response should include headers");
            var contentTypeHeader = _getAllResponse?.Headers
                .FirstOrDefault(h => h.Name?.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) ?? false);
            Assert.That(contentTypeHeader, Is.Not.Null, "Content-Type header should be present");
            Assert.That(contentTypeHeader?.Value, Does.Contain("application/json"),
                "Content-Type should be application/json");
        }

        [Then("the response should contain the correct region data")]
        public void ThenResponseShouldContainCorrectRegionData()
        {
            Assert.That(_getByIdResponse?.IsSuccessful, Is.True, "Response should be successful");
            Assert.That(_getByIdResponse?.Data, Is.Not.Null, "Region data should not be null");
            Assert.That(_getByIdResponse?.Data.Id, Is.EqualTo(_currentRegionId),
                "Returned region ID should match requested ID");
        }

        [Then("the region should have all required properties:")]
        public void ThenRegionShouldHaveAllRequiredProperties(Table table)
        {
            Assert.That(_getByIdResponse?.IsSuccessful, Is.True);
            Assert.That(_getByIdResponse?.Data, Is.Not.Null);

            var requiredProperties = table.Rows.Select(r => r["Property"]).ToList();
            var region = _getByIdResponse.Data;

            if (requiredProperties.Contains("Id"))
                Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty), "Region ID should not be empty");
            if (requiredProperties.Contains("Code"))
                Assert.That(region.Code, Is.Not.Null.And.Not.Empty, "Region Code should not be null or empty");
            if (requiredProperties.Contains("Name"))
                Assert.That(region.Name, Is.Not.Null.And.Not.Empty, "Region Name should not be null or empty");
        }

        [Then("the created region should be returned with a valid ID")]
        public void ThenCreatedRegionShouldHaveValidId()
        {
            var response = _scenarioContext.Get<RestSharp.RestResponse<RegionDto>>("createResponse");
            Assert.That(response.Data, Is.Not.Null, "Created region should be returned");
            Assert.That(response.Data.Id, Is.Not.EqualTo(Guid.Empty), "Created region should have valid ID");
        }

        [Then("the region data should match the created data")]
        public void ThenRegionDataShouldMatchCreatedData()
        {
            Assert.That(_getByIdResponse?.IsSuccessful, Is.True);
            Assert.That(_getByIdResponse?.Data.Code, Is.EqualTo(_currentRegionRequest.Code));
        }

        [Then("the update response should be successful")]
        public void ThenUpdateResponseShouldBeSuccessful()
        {
            var response = _scenarioContext.Get<RestSharp.RestResponse<RegionDto>>("updateResponse");
            Assert.That(response.IsSuccessful, Is.True, "Update should be successful");
        }

        [Then("the retrieved region should reflect the updated data")]
        public async Task ThenRetrievedRegionShouldReflectUpdatedData()
        {
            var getResponse = await _apiClient.GetRegionByIdAsync(_currentRegionId);
            Assert.That(getResponse.Data.Name, Is.EqualTo(_currentRegionRequest.Name),
                "Region should reflect updated name");
        }

        [Then("the delete response should be successful")]
        public void ThenDeleteResponseShouldBeSuccessful()
        {
            Assert.That(_deleteResponse?.IsSuccessful, Is.True, "Delete should be successful");
        }

        [Then("all concurrent requests should succeed")]
        public void ThenAllConcurrentRequestsShouldSucceed()
        {
            foreach (var task in _concurrentTasks)
            {
                var result = task.Result;
                Assert.That(result.IsSuccessful, Is.True, "All concurrent requests should succeed");
            }
        }

        [Then("all responses should contain regions")]
        public void ThenAllResponsesShouldContainRegions()
        {
            foreach (var task in _concurrentTasks)
            {
                var result = task.Result;
                Assert.That(result.Data, Is.Not.Empty, "All responses should contain regions");
            }
        }

        [Then("the API should handle the response gracefully")]
        public void ThenApiShouldHandleResponseGracefully()
        {
            Assert.That(_getAllResponse?.IsSuccessful, Is.True);
        }

        [Then("the response should contain a valid list")]
        public void ThenResponseShouldContainValidList()
        {
            Assert.That(_getAllResponse?.Data, Is.Not.Null, "Should return list rather than null");
            Assert.That(_getAllResponse?.Data, Is.InstanceOf<List<RegionDto>>(), "Should be a valid list");
        }

        [Then("each region from GetAll should match its individual GetById response")]
        public async Task ThenEachRegionShouldMatchIndividualResponse()
        {
            foreach (var region in _allRegions)
            {
                var getByIdResponse = await _apiClient.GetRegionByIdAsync(region.Id);
                Assert.That(getByIdResponse.IsSuccessful, Is.True);
                Assert.That(getByIdResponse.Data.Id, Is.EqualTo(region.Id));
                Assert.That(getByIdResponse.Data.Code, Is.EqualTo(region.Code));
                Assert.That(getByIdResponse.Data.Name, Is.EqualTo(region.Name));
            }
        }

        #endregion
    }
}
