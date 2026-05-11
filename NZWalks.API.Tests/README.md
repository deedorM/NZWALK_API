# NZWalks API Automated Test Framework

This is a comprehensive automated test framework for the NZWalks API using Playwright C# and RestSharp. The framework provides automated testing for the Regions API endpoints.

## Overview

The test framework is built using:
- **RestSharp** - For HTTP API requests
- **NUnit** - For test framework and assertions
- **Playwright** - For API testing capabilities (can be extended for UI testing)
- **.NET 9** - Target framework

## Project Structure

```
NZWalks.API.Tests/
??? ApiClients/
?   ??? RegionsApiClient.cs          # API client wrapper for making HTTP requests
??? Configuration/
?   ??? TestConfiguration.cs         # Configuration and settings for tests
??? Fixtures/
?   ??? BaseTestFixture.cs           # Base fixture class for common test setup
??? Tests/
?   ??? GetAllRegionsTests.cs        # Test cases for Get All Regions endpoint
?   ??? GetRegionByIdTests.cs        # Test cases for Get Region by ID endpoint
??? NZWalks.API.Tests.csproj         # Project file
??? README.md                        # This file
```

## Features

### GetAllRegionsTests
- Verify successful API response (Status 200)
- Verify response returns a list of regions
- Validate required properties on each region
- Verify valid RegionDTO structure
- Performance testing - response time validation
- Data consistency checks

### GetRegionByIdTests
- Verify successful API response for valid ID (Status 200)
- Verify correct region data is returned
- Verify 404 Not Found for non-existent regions
- Validate required properties on returned region
- Performance testing - response time validation
- Data consistency across multiple calls

## Setup & Configuration

### Prerequisites
- .NET 9 SDK or later
- Visual Studio 2022 or VS Code
- NZWalks API running locally or accessible via network

### Configuration

Tests use the following configuration sources (in order of precedence):

1. **Environment Variables:**
   - `API_BASE_URL` - Base URL for the API (default: `https://localhost:7070`)
   - `API_TIMEOUT_SECONDS` - Timeout for API calls in seconds (default: `30`)

2. **TestConfiguration Class:**
   - Can be modified in `Configuration/TestConfiguration.cs`

### Running Tests Locally

#### Option 1: Using Visual Studio
1. Build the solution
2. Open Test Explorer (Test > Test Explorer)
3. Select tests and click "Run Selected Tests"

#### Option 2: Using Command Line
```bash
# Run all tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# Run specific test class
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"

# Run with specific API base URL
set API_BASE_URL=https://localhost:7070
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# Run with verbose output
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj -v detailed
```

#### Option 3: Using .NET CLI
```bash
# From the solution root
cd NZWalks.API.Tests
dotnet test
```

## Test Cases

### Get All Regions Tests

#### Test 1: GetAllRegions_ShouldReturnOkStatus
- **Description:** Verify that GetAll API returns a successful response with status code 200
- **Expected:** Status code is 200 OK and response is successful

#### Test 2: GetAllRegions_ShouldReturnListOfRegions
- **Description:** Verify that GetAll API returns a list of regions
- **Expected:** Response contains a list of RegionDTO objects

#### Test 3: GetAllRegions_RegionsShouldHaveRequiredProperties
- **Description:** Verify that each region in the response has required properties
- **Expected:** Each region has Id, Code, and Name populated

#### Test 4: GetAllRegions_ShouldReturnValidRegionDtos
- **Description:** Verify that GetAll API returns valid Region DTOs with all properties
- **Expected:** All properties have correct types

#### Test 5: GetAllRegions_ResponseTimeShouldBeAcceptable
- **Description:** Verify that GetAll API response time is within acceptable limits
- **Expected:** Response time is less than configured timeout

### Get Region By ID Tests

#### Test 1: GetRegionById_WithValidId_ShouldReturnOkStatus
- **Description:** Verify that GetById API returns a successful response for a valid region ID
- **Expected:** Status code is 200 OK for valid ID

#### Test 2: GetRegionById_WithValidId_ShouldReturnCorrectRegionData
- **Description:** Verify that GetById API returns correct region data
- **Expected:** Returned region matches the expected region data

#### Test 3: GetRegionById_WithInvalidId_ShouldReturnNotFound
- **Description:** Verify that GetById API returns 404 Not Found for non-existent region
- **Expected:** Status code is 404 Not Found for invalid GUID

#### Test 4: GetRegionById_ShouldReturnRegionWithAllRequiredProperties
- **Description:** Verify that GetById API returns region with all required properties
- **Expected:** Returned region has all required properties populated

#### Test 5: GetRegionById_ResponseTimeShouldBeAcceptable
- **Description:** Verify that GetById API response time is within acceptable limits
- **Expected:** Response time is less than configured timeout

#### Test 6: GetRegionById_ShouldReturnConsistentData
- **Description:** Verify that GetById API returns consistent data across multiple calls
- **Expected:** Multiple calls return identical data

## Extending the Framework

### Adding New API Client Methods

To add new API endpoints to the test framework:

1. **Add method to `RegionsApiClient.cs`:**
```csharp
public async Task<RestResponse<YourDto>> YourMethodAsync(YourParam param)
{
    var request = new RestRequest("api/endpoint", Method.Post);
    request.AddJsonBody(param);
    return await _client.ExecuteAsync<YourDto>(request);
}
```

2. **Create new test class** following the naming pattern:
```csharp
[TestFixture]
public class YourTests : BaseTestFixture
{
    [Test]
    public async Task YourTestMethod()
    {
        // Arrange
        // Act
        var response = await ApiClient.YourMethodAsync(param);
        // Assert
    }
}
```

### Adding New Test Cases

Follow this template for new test cases:

```csharp
[Test]
[Description("Clear description of what is being tested")]
public async Task TestMethodName()
{
    // Arrange - Set up test data

    // Act - Execute the API call

    // Assert - Verify the results
}
```

## Best Practices

1. **Use meaningful test names** - Follow the pattern: `MethodName_Scenario_ExpectedBehavior`
2. **Add descriptions** - Use `[Description]` attribute for clarity
3. **Validate responses** - Always check status code, response data, and error messages
4. **Use assertions properly** - Be specific with assertion messages
5. **Clean up resources** - Override `TearDown()` method if needed for cleanup
6. **Handle async/await** - All API calls should be awaited properly
7. **Test both happy path and edge cases** - Include tests for success, failures, and boundary conditions

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Run API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: windows-latest

    steps:
    - uses: actions/checkout@v2

    - name: Setup .NET 9
      uses: actions/setup-dotnet@v2
      with:
        dotnet-version: 9.0.x

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release --no-restore

    - name: Run tests
      run: dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --no-build --verbosity normal
      env:
        API_BASE_URL: https://localhost:7070
```

## Troubleshooting

### Common Issues

#### 1. Tests fail with "Connection refused"
- Ensure the API is running
- Verify `API_BASE_URL` environment variable is set correctly
- Check firewall settings

#### 2. Tests timeout
- Increase `API_TIMEOUT_SECONDS` environment variable
- Check API performance
- Verify network connectivity

#### 3. Tests fail with "404 Not Found"
- Verify the API endpoints are correct
- Ensure sample data exists in the database
- Check the API routing configuration

#### 4. SSL/Certificate errors
- For development, accept self-signed certificates (handled by RestSharp by default)
- Ensure HTTPS is configured correctly

## Future Enhancements

- Add Playwright browser automation for UI testing
- Implement data-driven tests using CSV/JSON data sources
- Add API documentation validation tests
- Implement performance benchmarking tests
- Add negative test cases and error scenario testing
- Integrate with CI/CD pipelines
- Add test report generation
- Implement Allure reporting for better test visualization

## Support

For issues or questions about the test framework, please refer to:
- [RestSharp Documentation](https://restsharp.dev/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/)

## License

This test framework is part of the NZWalks project.
