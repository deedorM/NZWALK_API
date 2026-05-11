# NZWalks API Test Framework - Quick Reference Card

## ?? Start Here

```bash
# 1. Start the API
cd NZWalks.API
dotnet run

# 2. In another terminal, run tests
cd NZWalks.API.Tests
dotnet test
```

---

## ?? All Tests (27 Total)

### Category 1: GetAllRegionsTests (5)
- `GetAllRegions_ShouldReturnOkStatus`
- `GetAllRegions_ShouldReturnListOfRegions`
- `GetAllRegions_RegionsShouldHaveRequiredProperties`
- `GetAllRegions_ShouldReturnValidRegionDtos`
- `GetAllRegions_ResponseTimeShouldBeAcceptable`

### Category 2: GetRegionByIdTests (6)
- `GetRegionById_WithValidId_ShouldReturnOkStatus`
- `GetRegionById_WithValidId_ShouldReturnCorrectRegionData`
- `GetRegionById_WithInvalidId_ShouldReturnNotFound`
- `GetRegionById_ShouldReturnRegionWithAllRequiredProperties`
- `GetRegionById_ResponseTimeShouldBeAcceptable`
- `GetRegionById_ShouldReturnConsistentData`

### Category 3: RegionsApiIntegrationTests (6)
- `RegionsApi_CompleteCrudWorkflow`
- `RegionsApi_DataConsistencyBetweenEndpoints`
- `RegionsApi_ShouldReturnCorrectResponseHeaders`
- `RegionsApi_ShouldHandleConcurrentRequests`
- `RegionsApi_ShouldHandleEdgeCases`
- `RegionsApi_ShouldHandleSpecialCharactersInResponse`

### Category 4: RegionsApiDataDrivenTests (10)
- `GetAllRegions_VerifyConsistentStructure`
- `GetAllRegions_KnownRegionsShouldExist` [Parameterized]
- `RegionBuilder_ShouldBuildValidRegions`
- `RegionBuilder_ShouldGenerateRandomData`
- `TestDataSets_ShouldProvideValidData`
- `TestDataSets_AllRegionsShouldHaveData`
- `RegionBuilder_ShouldSupportChaining`
- `GetRegionById_WithBuilderData_ShouldWork`
- `GetAllRegions_RegionIdsShouldBeValidGuids`
- `GetRegionById_WithTestCases_ShouldReturnCorrectData` [Parameterized]

---

## ? Common Commands

| Command | Purpose |
|---------|---------|
| `dotnet test` | Run all tests |
| `dotnet test --filter "Name~GetAll"` | Run GetAll tests only |
| `dotnet test --filter "Name~GetById"` | Run GetById tests only |
| `dotnet test --filter "Name~Integration"` | Run integration tests |
| `dotnet test -v detailed` | Verbose output |
| `dotnet test --list-tests` | List all tests |

---

## ?? Filter Examples

```bash
# Run specific class
dotnet test --filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"

# Run tests containing pattern
dotnet test --filter "Name~Valid"

# Run single test
dotnet test --filter "Name=GetAllRegions_ShouldReturnOkStatus"

# Exclude certain tests
dotnet test --filter "ClassName!=RegionsApiIntegrationTests"

# Run with settings file
dotnet test --settings test.runsettings
```

---

## ?? Project Files

```
NZWalks.API.Tests/
??? ApiClients/RegionsApiClient.cs         ? API calls
??? Builders/RegionBuilder.cs              ? Test data
??? Configuration/TestConfiguration.cs     ? Settings
??? Fixtures/BaseTestFixture.cs            ? Base class
??? Helpers/TestHelper.cs                  ? Utilities
??? Tests/GetAllRegionsTests.cs            ? 5 tests
??? Tests/GetRegionByIdTests.cs            ? 6 tests
??? Tests/RegionsApiIntegrationTests.cs    ? 6 tests
??? Tests/RegionsApiDataDrivenTests.cs     ? 10 tests
??? README.md                              ? Full docs
??? QUICKSTART.md                          ? 5-min guide
??? TEST_EXECUTION_GUIDE.md                ? How to run
??? TEST_INDEX.md                          ? All tests
??? SETUP_COMPLETE.md                      ? Setup info
```

---

## ?? Test Data Sets Available

```csharp
// Pre-built test data
TestDataSets.AucklandRegion              // AKL - Auckland
TestDataSets.WellingtonRegion            // WLG - Wellington
TestDataSets.ChristchurchRegion          // CHC - Christchurch
TestDataSets.QueenstownRegion            // ZQN - Queenstown
TestDataSets.RegionWithSpecialCharacters // SPC - Special chars
TestDataSets.RegionWithLongName          // LNG - Long name
TestDataSets.RegionWithMinimalData       // MIN - Minimal
TestDataSets.AllTestRegions              // All combined
```

---

## ??? Builder Usage

```csharp
// Create custom test data
var region = new RegionBuilder()
    .WithCode("TST")
    .WithName("Test")
    .WithImageUrl("https://example.com/test.jpg")
    .BuildRegionDto();

// Or with random data
var randomRegion = new RegionBuilder()
    .WithRandomCode()
    .WithRandomName()
    .BuildRegionDto();

// For create requests
var createRequest = new RegionBuilder()
    .WithCode("NEW")
    .WithName("New Region")
    .BuildCreateRequest();
```

---

## ?? Configuration

```bash
# Via Environment Variables
$env:API_BASE_URL = "https://localhost:7070"
$env:API_TIMEOUT_SECONDS = "30"

# Then run
dotnet test
```

Or edit `Configuration/TestConfiguration.cs`:
```csharp
public static string ApiBaseUrl => "https://localhost:7070";
public static int TimeoutSeconds => 30;
```

---

## ?? API Client Methods

```csharp
// In your tests
var client = new RegionsApiClient(baseUrl);

// Get all regions
var allRegions = await client.GetAllRegionsAsync();

// Get specific region
var region = await client.GetRegionByIdAsync(id);

// Create region
var created = await client.CreateRegionAsync(data);

// Update region
var updated = await client.UpdateRegionAsync(id, data);

// Delete region
var deleted = await client.DeleteRegionAsync(id);
```

---

## ? Response Objects

```csharp
// Generic response
RestResponse<T> response

// Contains
response.StatusCode        // HTTP status code
response.IsSuccessful      // true/false
response.Data             // T (deserialized object)
response.Content          // Raw response body
response.Headers          // Response headers
response.ErrorMessage     // Error if any

// For regions
RegionDto region
region.Id                 // Guid
region.Code              // string
region.Name              // string
region.RegionImageUrl    // string
```

---

## ?? Common Assertions

```csharp
// Status codes
Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
Assert.That(response.IsSuccessful, Is.True);

// Data validation
Assert.That(response.Data, Is.Not.Null);
Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty));
Assert.That(region.Code, Is.Not.Null.And.Not.Empty);

// Lists
Assert.That(regions, Is.Not.Empty);
Assert.That(regions.Count, Is.GreaterThan(0));

// Type checking
Assert.That(region, Is.TypeOf<RegionDto>());

// Performance
Assert.That(responseTime.TotalSeconds, Is.LessThan(30));
```

---

## ?? CI/CD Quick Setup

```bash
# GitHub Actions example
# Create .github/workflows/test.yml

name: Tests
on: [push, pull_request]
jobs:
  test:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v2
        with:
          dotnet-version: 9.0.x
      - run: dotnet test
        env:
          API_BASE_URL: https://localhost:7070
```

---

## ?? Troubleshooting Quick Fixes

| Issue | Fix |
|-------|-----|
| "Connection refused" | Start API with `dotnet run` |
| "Certificate error" | Normal for dev HTTPS, ignore |
| "404 Not Found" | Check API_BASE_URL |
| "Tests not found" | Run `dotnet clean` then `dotnet build` |
| "Timeout" | Increase API_TIMEOUT_SECONDS |

---

## ?? Documentation Files

| File | Content |
|------|---------|
| `README.md` | Complete documentation |
| `QUICKSTART.md` | 5-minute setup |
| `TEST_INDEX.md` | All test details |
| `TEST_EXECUTION_GUIDE.md` | How to run tests |
| `IMPLEMENTATION_SUMMARY.md` | Technical details |
| `SETUP_COMPLETE.md` | Setup summary |
| `QUICK_REFERENCE.md` | This file |

---

## ?? Example Test Cases

### Simple Test
```csharp
[Test]
public async Task GetAllRegions_ShouldReturnOkStatus()
{
    var response = await ApiClient.GetAllRegionsAsync();
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
}
```

### With Assertions
```csharp
[Test]
public async Task GetAllRegions_ShouldReturnListOfRegions()
{
    var response = await ApiClient.GetAllRegionsAsync();
    Assert.That(response.IsSuccessful, Is.True);
    Assert.That(response.Data, Is.Not.Null);
    Assert.That(response.Data, Is.InstanceOf<List<RegionDto>>());
}
```

### With Setup
```csharp
[Test]
public async Task GetRegionById_WithValidId_ShouldReturnOkStatus()
{
    var allResponse = await ApiClient.GetAllRegionsAsync();
    var validId = allResponse.Data.First().Id;

    var response = await ApiClient.GetRegionByIdAsync(validId);
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
}
```

---

## ?? You're All Set!

? Framework installed
? 27 tests ready
? Complete documentation
? Full API coverage
? Ready for production

**Start testing:**
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj
```

---

**Quick Links:**
- Full Setup: See `SETUP_COMPLETE.md`
- All Tests: See `TEST_INDEX.md`
- Documentation: See `README.md`
- How to Run: See `TEST_EXECUTION_GUIDE.md`

---

**Status:** ? Ready to Use
**Tests:** 27
**Framework:** .NET 9
**Last Updated:** 2024
