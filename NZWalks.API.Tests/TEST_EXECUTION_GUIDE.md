# NZWalks API Test Execution Script

This script helps run tests with various configurations.

## PowerShell Usage (Windows)

### Run All Tests
```powershell
# Simple run
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# With verbose output
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --verbosity detailed

# Save results to file
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "trx;LogFileName=results.trx"
```

### Run Specific Test Classes
```powershell
# GetAllRegionsTests only
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"

# GetRegionByIdTests only
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.GetRegionByIdTests"

# Integration tests only
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.RegionsApiIntegrationTests"

# Data-driven tests only
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.RegionsApiDataDrivenTests"
```

### Run Specific Tests by Name
```powershell
# Run single test
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name=GetAllRegions_ShouldReturnOkStatus"

# Run tests matching pattern
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~GetAllRegions"
```

### With Custom API URL
```powershell
# Set environment variable
$env:API_BASE_URL = "https://localhost:7070"

# Run tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj

# Clear environment variable
Remove-Item Env:\API_BASE_URL
```

### List All Tests
```powershell
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --list-tests
```

### Run Tests in Release Mode
```powershell
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --configuration Release
```

## Batch Script Usage (Windows - .bat)

Create a file `run-tests.bat`:

```batch
@echo off
REM NZWalks API Test Execution Script

setlocal enabledelayedexpansion

REM Set default API URL
if "%API_BASE_URL%"=="" (
    set API_BASE_URL=https://localhost:7070
    echo API_BASE_URL not set. Using default: !API_BASE_URL!
) else (
    echo Using API_BASE_URL: !API_BASE_URL!
)

echo.
echo Running NZWalks API Tests...
echo.

REM Run all tests
dotnet test NZWalks.API.Tests\NZWalks.API.Tests.csproj --verbosity normal

echo.
echo Test run completed!
pause
```

Run with: `.\run-tests.bat`

## Bash Script Usage (Linux/Mac)

Create a file `run-tests.sh`:

```bash
#!/bin/bash

# NZWalks API Test Execution Script

# Set default API URL
if [ -z "$API_BASE_URL" ]; then
    export API_BASE_URL="https://localhost:7070"
    echo "API_BASE_URL not set. Using default: $API_BASE_URL"
else
    echo "Using API_BASE_URL: $API_BASE_URL"
fi

echo ""
echo "Running NZWalks API Tests..."
echo ""

# Run all tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --verbosity normal

echo ""
echo "Test run completed!"
```

Run with: `chmod +x run-tests.sh && ./run-tests.sh`

## GitHub Actions Workflow

Create `.github/workflows/run-tests.yml`:

```yaml
name: NZWalks API Tests

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
      run: dotnet build --no-restore --configuration Release

    - name: Run Tests
      run: dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx"
      env:
        API_BASE_URL: https://localhost:7070

    - name: Upload Test Results
      if: always()
      uses: actions/upload-artifact@v2
      with:
        name: test-results
        path: '**/TestResults/'
```

## Visual Studio Test Explorer

1. **Open Test Explorer:**
   - Test ? Test Explorer
   - Or: Ctrl + E, T

2. **Run Tests:**
   - Click "Run All Tests" button
   - Or select specific tests and click "Run Selected Tests"
   - Or right-click test and select "Run Tests"

3. **View Results:**
   - Green checkmark = Passed
   - Red X = Failed
   - Yellow warning = Skipped

4. **Debug Tests:**
   - Right-click test and select "Debug Test"

## Filter Examples

| Filter | Command |
|--------|---------|
| All tests | `dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj` |
| GetAll* tests | `dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~GetAll"` |
| Tests with "Valid" | `dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "Name~Valid"` |
| Excluding integration | `dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName!=RegionsApiIntegrationTests"` |
| Specific class | `dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --filter "ClassName=NZWalks.API.Tests.Tests.GetAllRegionsTests"` |

## Test Result Logging

### Console Output
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --verbosity detailed
```

### TRX Format (Visual Studio)
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "trx;LogFileName=results.trx"
```

### XUnit Format
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "xunit;LogFileName=results.xml"
```

### Multiple Formats
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "trx" --logger "console;verbosity=detailed"
```

## Troubleshooting

### Connection Refused
```bash
# Check if API is running
curl https://localhost:7070/api/regions -k

# If not, start the API
cd NZWalks.API
dotnet run
```

### Certificate Issues
```bash
# Set environment to Development
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj
```

### Tests Not Found
```bash
# List all available tests
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --list-tests

# Rebuild project
dotnet clean NZWalks.API.Tests/NZWalks.API.Tests.csproj
dotnet build NZWalks.API.Tests/NZWalks.API.Tests.csproj
```

## Performance Profiling

### Run with Timing
```bash
dotnet test NZWalks.API.Tests/NZWalks.API.Tests.csproj --logger "console;verbosity=detailed" --diag diagnostics.zip
```

## Continuous Testing

### Watch Mode (Requires dotnet-watch)
```bash
# Install watch tool
dotnet tool install -g dotnet-watch

# Run tests in watch mode
dotnet watch test NZWalks.API.Tests/NZWalks.API.Tests.csproj
```

---

For more information, see README.md and QUICKSTART.md in the test project.
