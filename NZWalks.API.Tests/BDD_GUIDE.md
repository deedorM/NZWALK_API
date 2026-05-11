# NZWalks API - BDD Test Suite

This document provides an overview of the BDD (Behavior-Driven Development) test suite for the NZWalks API.

## Overview

The test project has been converted to use BDD with Reqnroll (modern replacement for SpecFlow) and NUnit. All test scenarios are written in Gherkin language and implemented with step definitions.

## Project Structure

```
NZWalks.API.Tests/
├── Features/                          # Gherkin feature files
│   ├── GetAllRegions.feature         # Scenarios for retrieving all regions
│   ├── GetRegionById.feature         # Scenarios for retrieving a region by ID
│   └── RegionsCrud.feature           # Scenarios for CRUD operations
├── StepDefinitions/
│   └── RegionsStepDefinitions.cs     # Step implementations for all scenarios
├── ApiClients/
│   └── RegionsApiClient.cs           # API client for making HTTP requests
├── Builders/
│   └── RegionBuilder.cs              # Test data builders
├── Configuration/
│   └── TestConfiguration.cs          # Test configuration and settings
├── Fixtures/
│   └── BaseTestFixture.cs            # Base test fixture setup
├── Helpers/
│   └── TestHelper.cs                 # Utility helpers
└── Tests/                            # Legacy unit tests (can be migrated to BDD)
    ├── GetAllRegionsTests.cs
    ├── GetRegionByIdTests.cs
    ├── RegionsApiIntegrationTests.cs
    └── RegionsApiDataDrivenTests.cs
```

## Feature Files

### 1. GetAllRegions.feature
Covers scenarios for the GetAll regions endpoint:
- Successfully retrieve all regions with 200 OK status
- Verify each region has required properties (Id, Code, Name)
- Verify response contains valid Region DTOs
- Verify response time is acceptable (<5 seconds)
- Verify response has correct Content-Type header

### 2. GetRegionById.feature
Covers scenarios for the GetRegionById endpoint:
- Successfully retrieve a region with valid ID
- Return 404 Not Found for non-existent region
- Verify region has all required properties
- Verify response time is acceptable
- Verify data consistency between GetAll and GetById

### 3. RegionsCrud.feature
Covers scenarios for Create, Read, Update, Delete operations:
- Complete CRUD workflow (Create → Read → Update → Delete)
- Create region returns 201 Created
- Concurrent requests handling
- Edge cases handling
- Data consistency across endpoints

## Running Tests

### Prerequisites
1. Ensure the NZWalks.API is running on the configured base URL (default: http://localhost:5000)
2. The database should be initialized with data

### Running All Tests
```bash
cd NZWalks.API.Tests
dotnet test
```

### Running Specific Feature Tests
```bash
# Run all GetAllRegions tests
dotnet test --filter "Name~GetAllRegions"

# Run all GetRegionById tests
dotnet test --filter "Name~GetRegionById"

# Run CRUD operation tests
dotnet test --filter "Name~RegionsCrud"
```

### Running with Verbose Output
```bash
dotnet test -v detailed
```

### Listing All Available Tests
```bash
dotnet test --list-tests
```

## Understanding Step Definitions

The step definitions in `RegionsStepDefinitions.cs` are organized into three categories:

### Given Steps (Setup/Arrange)
- `Given I have obtained a valid region ID from the GetAll endpoint`
- `Given I have retrieved all regions`
- `Given I have a new region to create with:`

### When Steps (Action/Act)
- `When I call the GetAll regions endpoint`
- `When I call the GetRegion by ID endpoint with that ID`
- `When I create the region`
- `When I delete the region`
- etc.

### Then Steps (Verification/Assert)
- `Then the response status code should be 200 OK`
- `Then the response should contain a list of regions`
- `Then the response time should be less than <seconds> seconds`
- etc.

## Test Configuration

The test configuration is located in `Configuration/TestConfiguration.cs` and includes:
- `ApiBaseUrl`: Base URL for the API (default: http://localhost:5000)
- `TimeoutSeconds`: Timeout for API responses (default: 5 seconds)

## Test Data Builders

The `Builders/RegionBuilder.cs` provides a fluent interface for building test data:
```csharp
var region = new RegionBuilder()
    .WithCode("NZ001")
    .WithName("Test Region")
    .WithImageUrl("https://example.com/image.jpg")
    .Build();
```

## API Client

The `RegionsApiClient.cs` provides methods for API operations:
- `GetAllRegionsAsync()` - Get all regions
- `GetRegionByIdAsync(id)` - Get region by ID
- `CreateRegionAsync(request)` - Create new region
- `UpdateRegionAsync(id, request)` - Update region
- `DeleteRegionAsync(id)` - Delete region

## Best Practices

1. **Feature Organization**: Each feature file focuses on a specific API endpoint or workflow
2. **Scenario Clarity**: Scenarios are written in plain English that describes the business behavior
3. **Step Reusability**: Steps are designed to be reusable across multiple scenarios
4. **Test Data**: Use builders for consistent test data generation
5. **Assertions**: Use NUnit assertions for clear error messages

## Troubleshooting

### Tests fail with "Connection refused"
- Ensure the NZWalks.API is running
- Check the `ApiBaseUrl` in `TestConfiguration.cs`

### Tests fail with "Response status 500"
- Check the API logs for errors
- Ensure the database is properly initialized

### Test execution times out
- Increase `TimeoutSeconds` in `TestConfiguration.cs`
- Check network connectivity
- Review API server performance

## Next Steps

1. Run the full test suite to verify the BDD setup
2. Add additional scenarios as needed
3. Integrate with CI/CD pipeline
4. Set up reporting and metrics

## References

- [Reqnroll Documentation](https://reqnroll.net/)
- [Gherkin Syntax](https://cucumber.io/docs/gherkin/reference/)
- [NUnit Documentation](https://docs.nunit.org/)
