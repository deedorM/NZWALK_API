# Quick Start Guide - NZWalks API Test Framework

## 5-Minute Setup

### Step 1: Prerequisites
- Ensure .NET 9 SDK is installed
- Visual Studio 2022 or VS Code installed
- NZWalks API project accessible

### Step 2: Add Test Project to Solution
The test project has been created at: `NZWalks.API.Tests/`

This is automatically part of your solution and should appear in Solution Explorer.

### Step 3: Configure API URL
Set your API base URL in one of these ways:

**Option A: Environment Variable (Recommended)**
```powershell
# Set for current session
$env:API_BASE_URL = "https://localhost:7070"

# Verify
Write-Host $env:API_BASE_URL
```

**Option B: Run Settings File**
Edit `NZWalks.API.Tests\test.runsettings` and update:
```xml
<API_BASE_URL>https://localhost:7070</API_BASE_URL>
```

**Option C: Modify TestConfiguration.cs**
Edit `Configuration\TestConfiguration.cs` to change default values.

### Step 4: Start the API
Open a terminal and run:
```bash
# Navigate to API project
cd NZWalks.API

# Run the API
dotnet run
```

The API should be accessible at: `https://localhost:7070`

### Step 5: Run Tests

**Using Visual Studio:**
1. Build ? Build Solution
2. Test ? Test Explorer
3. Select tests and click "Run"

**Using Command Line:**
```bash
# From solution root
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# With verbose output
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj -v detailed

# Run specific test class
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"

# Run specific test
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name=GetAllRegions_ShouldReturnOkStatus"
```

## Available Test Suites

### 1. GetAllRegionsTests (5 tests)
Tests the `/api/regions` GET endpoint
- Successful response
- Response structure
- Data validation
- Performance
- Data consistency

### 2. GetRegionByIdTests (6 tests)
Tests the `/api/regions/{id}` GET endpoint
- Valid ID handling
- Data accuracy
- 404 handling for invalid IDs
- Data validation
- Performance
- Consistency across calls

### 3. RegionsApiIntegrationTests (6 tests)
Advanced integration scenarios
- Complete CRUD workflow
- Data consistency across endpoints
- Response headers validation
- Concurrent requests
- Edge case handling
- Special character handling

## What's Tested?

? API response status codes (200, 201, 404, etc.)
? Response data structure and types
? Required fields validation
? Data consistency
? Performance/Response time
? Error handling (404 Not Found)
? Concurrent request handling
? CRUD operations workflow

## Test Results

After running tests, you'll see results like:
```
Test Passed: GetAllRegions_ShouldReturnOkStatus
Test Passed: GetAllRegions_ShouldReturnListOfRegions
Test Passed: GetAllRegions_RegionsShouldHaveRequiredProperties
...
```

Green checkmarks = All tests passed! ?

## Troubleshooting

### "Connection refused" or "Connection timeout"
```bash
# Verify API is running
curl https://localhost:7070/api/regions -k

# If not running, start the API
cd NZWalks.API
dotnet run
```

### Tests show "Certificate error"
For development/HTTPS issues:
- This is expected for self-signed certificates
- RestSharp handles this automatically
- If issues persist, set: `ASPNETCORE_ENVIRONMENT=Development`

### Tests can't find API
Check API URL in test results:
```bash
# Set correct URL
$env:API_BASE_URL = "https://localhost:7070"
```

### Build errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## Next Steps

1. **Explore test files** to understand test structure
2. **Run tests regularly** - add to your development workflow
3. **Add more tests** for other API endpoints
4. **Integrate with CI/CD** - see README.md for GitHub Actions example
5. **Extend framework** - customize for your needs

## Useful Commands

```bash
# List all tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --list-tests

# Run tests with detailed output
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "console;verbosity=detailed"

# Generate test report
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "trx;LogFileName=test-results.trx"

# Run with specific configuration
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --configuration Release
```

## Documentation

- Full documentation: See `README.md` in the test project
- Test Configuration: Edit `Configuration/TestConfiguration.cs`
- API Client: See `ApiClients/RegionsApiClient.cs`
- Helpers: See `Helpers/TestHelper.cs` for utility functions

---

**For questions or support, refer to:**
- [RestSharp Docs](https://restsharp.dev/)
- [NUnit Docs](https://docs.nunit.org/)
- [Playwright Docs](https://playwright.dev/dotnet/)
