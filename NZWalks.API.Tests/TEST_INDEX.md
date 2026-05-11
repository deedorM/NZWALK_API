# NZWalks API Test Framework - Complete Test Index

## ?? Test Suite Summary

**Total Tests:** 27
**Test Classes:** 4
**Test Categories:** Unit Tests, Integration Tests, Data-Driven Tests

---

## ?? Test Classes & Cases

### 1. GetAllRegionsTests (5 tests)
**File:** `Tests/GetAllRegionsTests.cs`
**Endpoint:** `GET /api/regions`

| # | Test Name | Description | Status |
|---|-----------|-------------|--------|
| 1 | `GetAllRegions_ShouldReturnOkStatus` | Verify API returns 200 OK | ? |
| 2 | `GetAllRegions_ShouldReturnListOfRegions` | Verify response is a list | ? |
| 3 | `GetAllRegions_RegionsShouldHaveRequiredProperties` | Verify all regions have required fields | ? |
| 4 | `GetAllRegions_ShouldReturnValidRegionDtos` | Verify correct DTO types | ? |
| 5 | `GetAllRegions_ResponseTimeShouldBeAcceptable` | Verify performance | ? |

**What's Tested:**
- HTTP status code validation
- Response structure
- Data completeness
- Type correctness
- Performance metrics

---

### 2. GetRegionByIdTests (6 tests)
**File:** `Tests/GetRegionByIdTests.cs`
**Endpoint:** `GET /api/regions/{id}`

| # | Test Name | Description | Status |
|---|-----------|-------------|--------|
| 1 | `GetRegionById_WithValidId_ShouldReturnOkStatus` | Valid ID returns 200 | ? |
| 2 | `GetRegionById_WithValidId_ShouldReturnCorrectRegionData` | Correct data for valid ID | ? |
| 3 | `GetRegionById_WithInvalidId_ShouldReturnNotFound` | Invalid ID returns 404 | ? |
| 4 | `GetRegionById_ShouldReturnRegionWithAllRequiredProperties` | All fields present | ? |
| 5 | `GetRegionById_ResponseTimeShouldBeAcceptable` | Performance validation | ? |
| 6 | `GetRegionById_ShouldReturnConsistentData` | Consistency across calls | ? |

**What's Tested:**
- Valid ID handling
- 404 error handling
- Data accuracy
- Data consistency
- Performance
- Error scenarios

---

### 3. RegionsApiIntegrationTests (6 tests)
**File:** `Tests/RegionsApiIntegrationTests.cs`
**Type:** Integration Tests

| # | Test Name | Description | Status |
|---|-----------|-------------|--------|
| 1 | `RegionsApi_CompleteCrudWorkflow` | Create ? Read ? Update cycle | ? |
| 2 | `RegionsApi_DataConsistencyBetweenEndpoints` | GetAll vs GetById consistency | ? |
| 3 | `RegionsApi_ShouldReturnCorrectResponseHeaders` | Header validation | ? |
| 4 | `RegionsApi_ShouldHandleConcurrentRequests` | Multiple simultaneous calls | ? |
| 5 | `RegionsApi_ShouldHandleEdgeCases` | Edge case handling | ? |
| 6 | `RegionsApi_ShouldHandleSpecialCharactersInResponse` | Special character support | ? |

**What's Tested:**
- Complete CRUD workflow
- Cross-endpoint consistency
- Response headers
- Concurrent operations
- Edge cases
- Special characters

---

### 4. RegionsApiDataDrivenTests (10 tests)
**File:** `Tests/RegionsApiDataDrivenTests.cs`
**Type:** Data-Driven & Parameterized Tests

| # | Test Name | Description | Parameters | Status |
|---|-----------|-------------|------------|--------|
| 1 | `GetAllRegions_VerifyConsistentStructure` | Structure consistency | N/A | ? |
| 2 | `GetAllRegions_KnownRegionsShouldExist` | Known regions verification | "AKL", "WLG", "CHC" | ? |
| 3 | `RegionBuilder_ShouldBuildValidRegions` | Builder pattern validation | N/A | ? |
| 4 | `RegionBuilder_ShouldGenerateRandomData` | Random data generation | N/A | ? |
| 5 | `TestDataSets_ShouldProvideValidData` | Test data validation | N/A | ? |
| 6 | `TestDataSets_AllRegionsShouldHaveData` | Data completeness | N/A | ? |
| 7 | `RegionBuilder_ShouldSupportChaining` | Builder chaining | N/A | ? |
| 8 | `GetRegionById_WithBuilderData_ShouldWork` | GetById with builder | N/A | ? |
| 9 | `GetAllRegions_RegionIdsShouldBeValidGuids` | GUID validation | N/A | ? |
| 10 | `GetRegionById_WithTestCases_ShouldReturnCorrectData` | Parameterized GetById | "AKL", "WLG" | ? |

**What's Tested:**
- Data structure consistency
- Parameterized testing
- Builder pattern
- Test data management
- GUID validation
- Parameterized scenarios

---

## ?? Test Scenarios Covered

### API Response Scenarios
- ? 200 OK responses
- ? 201 Created responses
- ? 404 Not Found responses
- ? Response headers
- ? Content-Type validation

### Data Validation Scenarios
- ? Required field presence
- ? Data type validation
- ? Data format validation
- ? GUID format validation
- ? Special character handling
- ? Long string handling
- ? Minimal data handling

### Performance Scenarios
- ? Response time validation
- ? Concurrent request handling
- ? Multiple sequential calls

### Data Consistency Scenarios
- ? GetAll vs GetById consistency
- ? Multiple calls consistency
- ? CRUD workflow consistency

### Edge Cases
- ? Empty results
- ? Invalid IDs
- ? Non-existent resources
- ? Special characters
- ? Long values

---

## ?? Test Assertions by Type

### HTTP Status Assertions
```csharp
Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK))
Assert.That(response.IsSuccessful, Is.True)
```

### Data Structure Assertions
```csharp
Assert.That(response.Data, Is.Not.Null)
Assert.That(response.Data, Is.InstanceOf<List<RegionDto>>())
```

### Field Validation Assertions
```csharp
Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty))
Assert.That(region.Code, Is.Not.Null.And.Not.Empty)
```

### Performance Assertions
```csharp
Assert.That(responseTime.TotalSeconds, Is.LessThan(30))
```

### Consistency Assertions
```csharp
Assert.That(response1.Data.Id, Is.EqualTo(response2.Data.Id))
```

---

## ?? Test Execution Filters

### Run All Tests
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj
```

### Run by Class
```bash
# GetAllRegionsTests
--filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"

# GetRegionByIdTests
--filter "ClassName=NZWalks.API.Tests.Tests.GetRegionByIdTests"

# Integration Tests
--filter "ClassName=NZWalks.API.Tests.Tests.RegionsApiIntegrationTests"

# Data-Driven Tests
--filter "ClassName=NZWalks.API.Tests.Tests.RegionsApiDataDrivenTests"
```

### Run by Test Pattern
```bash
# All "GetAll" tests
--filter "Name~GetAll"

# All "GetById" tests
--filter "Name~GetById"

# All integration tests
--filter "Name~Integration"

# All builder tests
--filter "Name~Builder"
```

### Run Specific Test
```bash
--filter "Name=GetAllRegions_ShouldReturnOkStatus"
```

---

## ??? Helper Functions Used in Tests

| Helper Function | Purpose | Tests Using It |
|-----------------|---------|-----------------|
| `CreateSampleRegion()` | Create test region data | GetAll, GetById, Integration |
| `GenerateUniqueTestData()` | Generate unique test values | Integration tests |
| `CompareObjects<T>()` | Compare objects field-by-field | Integration tests |
| `RetryAsync()` | Retry failed operations | Could be used |
| `WaitForCondition()` | Wait for async conditions | Could be used |

---

## ?? Test Data Sets

| Data Set | Region Code | Region Name | Purpose |
|----------|------------|------------|---------|
| `AucklandRegion` | AKL | Auckland | Testing known region |
| `WellingtonRegion` | WLG | Wellington | Testing known region |
| `ChristchurchRegion` | CHC | Christchurch | Testing known region |
| `QueenstownRegion` | ZQN | Queenstown | Testing known region |
| `RegionWithSpecialCharacters` | SPC | Special chars | Testing special chars |
| `RegionWithLongName` | LNG | Long name | Testing long values |
| `RegionWithMinimalData` | MIN | M | Testing minimal data |

---

## ? Test Coverage Matrix

| Endpoint | Method | Create | Read | Update | Delete | Error |
|----------|--------|--------|------|--------|--------|-------|
| `/api/regions` | GET | ? | ? | - | - | - |
| `/api/regions/{id}` | GET | ? | ? | - | - | ? |
| `/api/regions` | POST | ? | ? | - | - | - |
| `/api/regions/{id}` | PUT | ? | ? | ? | - | - |
| `/api/regions/{id}` | DELETE | - | - | - | ? | - |

---

## ?? Test Examples

### Example 1: Simple Unit Test
```csharp
[Test]
public async Task GetAllRegions_ShouldReturnOkStatus()
{
    // Act
    var response = await ApiClient.GetAllRegionsAsync();

    // Assert
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    Assert.That(response.IsSuccessful, Is.True);
}
```

### Example 2: Parameterized Test
```csharp
[TestCase("AKL", "Auckland")]
[TestCase("WLG", "Wellington")]
[Test]
public async Task GetAllRegions_KnownRegionsShouldExist(string code, string name)
{
    // Arrange & Act & Assert
}
```

### Example 3: Builder Pattern Test
```csharp
[Test]
public void RegionBuilder_ShouldBuildValidRegions()
{
    var region = new RegionBuilder()
        .WithCode("TEST")
        .WithRandomName()
        .BuildRegionDto();

    Assert.That(region.Code, Is.EqualTo("TEST"));
}
```

### Example 4: Integration Test
```csharp
[Test]
public async Task RegionsApi_CompleteCrudWorkflow()
{
    // CREATE
    var createResponse = await ApiClient.CreateRegionAsync(request);
    var createdId = createResponse.Data.Id;

    // READ
    var getResponse = await ApiClient.GetRegionByIdAsync(createdId);

    // UPDATE
    var updateResponse = await ApiClient.UpdateRegionAsync(createdId, request);

    // VERIFY
    Assert.That(updateResponse.Data.Name, Is.EqualTo(request.Name));
}
```

---

## ?? Quick Test Commands

```bash
# Run all 27 tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# Run only GetAll tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~GetAll"

# Run only GetById tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~GetById"

# Run only integration tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~Integration"

# Run with verbose output
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj -v detailed

# List all tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --list-tests
```

---

## ?? Documentation References

- **Full Documentation:** `README.md`
- **Quick Start:** `QUICKSTART.md`
- **Execution Guide:** `TEST_EXECUTION_GUIDE.md`
- **Implementation Summary:** `IMPLEMENTATION_SUMMARY.md`
- **This Index:** `TEST_INDEX.md`

---

## ?? Next Steps

1. ? Review test structure
2. ? Run tests with: `dotnet test`
3. ? Examine test results
4. ? Extend with additional endpoints
5. ? Integrate into CI/CD pipeline

---

**Total Test Framework Coverage:** Comprehensive ?
**Ready for Production:** Yes ?
**Extensible Architecture:** Yes ?
**CI/CD Ready:** Yes ?
