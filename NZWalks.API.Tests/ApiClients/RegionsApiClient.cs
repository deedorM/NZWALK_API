using RestSharp;

namespace NZWalks.API.Tests.ApiClients
{
    public class RegionsApiClient
    {
        private readonly RestClient _client;

        public RegionsApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Get all regions from the API
        /// </summary>
        public async Task<RestResponse<List<RegionDto>>> GetAllRegionsAsync()
        {
            var request = new RestRequest("api/regions", Method.Get);
            return await _client.ExecuteAsync<List<RegionDto>>(request);
        }

        /// <summary>
        /// Get a region by ID
        /// </summary>
        public async Task<RestResponse<RegionDto>> GetRegionByIdAsync(Guid id)
        {
            var request = new RestRequest($"api/regions/{id}", Method.Get);
            return await _client.ExecuteAsync<RegionDto>(request);
        }

        /// <summary>
        /// Create a new region
        /// </summary>
        public async Task<RestResponse<RegionDto>> CreateRegionAsync(CreateRegionRequest regionRequest)
        {
            var request = new RestRequest("api/regions", Method.Post);
            request.AddJsonBody(regionRequest);
            return await _client.ExecuteAsync<RegionDto>(request);
        }

        /// <summary>
        /// Update an existing region
        /// </summary>
        public async Task<RestResponse<RegionDto>> UpdateRegionAsync(Guid id, CreateRegionRequest regionRequest)
        {
            var request = new RestRequest($"api/regions/{id}", Method.Put);
            request.AddJsonBody(regionRequest);
            return await _client.ExecuteAsync<RegionDto>(request);
        }

        /// <summary>
        /// Delete a region
        /// </summary>
        public async Task<RestResponse> DeleteRegionAsync(Guid id)
        {
            var request = new RestRequest($"api/regions/{id}", Method.Delete);
            return await _client.ExecuteAsync(request);
        }
    }

    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }

    public class CreateRegionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}
