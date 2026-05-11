# BDD Quick Start Guide

## 🚀 Get Started in 5 Minutes

### 1. Start the API
```bash
cd NZWalks.API
dotnet run
# API will start on http://localhost:5029
```

### 2. Run BDD Tests (in another terminal)
```bash
cd NZWalks.API.Tests
dotnet test
```

That's it! 🎉

## 📋 Understanding the BDD Structure

Each BDD scenario follows the **Given-When-Then** pattern:

```gherkin
Scenario: Successfully retrieve all regions
    When I call the GetAll regions endpoint
    Then the response status code should be 200 OK
    And the response should contain a list of regions
```

### Breaking It Down
- **Given** - Setup/preconditions (not needed for all scenarios)
- **When** - Action/what we're testing
- **Then** - Expected result/assertions
- **And** - Additional conditions (When or Then)

## 🗂️ Where Are the Tests?

### Feature Files (Human-Readable)
```
Features/
├── GetAllRegions.feature      # Scenarios for retrieving all regions
├── GetRegionById.feature       # Scenarios for retrieving by ID
└── RegionsCrud.feature         # Create/Update/Delete scenarios
```

### Step Implementations (Code)
```
StepDefinitions/
└── RegionsStepDefinitions.cs  # All step definitions
```

## 🏃 Common Commands

### Run All Tests
```bash
dotnet test
```

### Run Specific Feature
```bash
# GetAll tests only
dotnet test --filter "ClassName~GetAllRegions"

# GetById tests only
dotnet test --filter "ClassName~GetRegionById"

# CRUD tests only
dotnet test --filter "ClassName~RegionsCrud"
```

### Run Single Test
```bash
dotnet test --filter "Name~SuccessfullyRetrieveAllRegions"
```

### See Detailed Output
```bash
dotnet test -v detailed
```

### List All Available Tests
```bash
dotnet test --list-tests
```

## 📝 Creating a New BDD Scenario

### 1. Add Scenario to Feature File
Edit `Features/GetAllRegions.feature`:
```gherkin
Scenario: Verify API returns regions sorted by name
    When I call the GetAll regions endpoint
    Then the regions should be sorted by name
    And the response should be consistent
```

### 2. Add Step Definitions to RegionsStepDefinitions.cs
```csharp
[Then("the regions should be sorted by name")]
public void ThenRegionsShouldBeSortedByName()
{
    var sortedRegions = _getAllResponse.Data
        .OrderBy(r => r.Name)
        .ToList();
    
    Assert.That(_getAllResponse.Data, Is.EqualTo(sortedRegions),
        "Regions should be sorted by name");
}
```

### 3. Run Tests
```bash
dotnet test --filter "Name~SortedByName"
```

## 🔍 Test Examples

### Example 1: Simple Assertion
```gherkin
Scenario: API returns valid responses
    When I call the GetAll regions endpoint
    Then the response status code should be 200 OK
```

### Example 2: With Data Table
```gherkin
Scenario: Create region with data
    Given I have a new region to create with:
        | Code      | Name           |
        | TEST_CODE | Test Region    |
    When I create the region
    Then the response status code should be 201 Created
```

### Example 3: Complex Workflow
```gherkin
Scenario: Complete CRUD workflow
    Given I have a new region to create with:
        | Code | TEMP |
        | Name | Temporary Region |
    When I create the region
    Then the created region should be returned with a valid ID
    When I retrieve the created region by ID
    Then the region data should match the created data
    When I delete the region
    Then the delete response should be successful
```

## 🐛 Troubleshooting

### Tests Fail with Connection Refused
```
Issue: Unable to connect to API
Solution: Make sure the API is running on http://localhost:5029
```

### Tests Timeout
```
Issue: Tests take too long or timeout
Solution: Check network connectivity or API performance
```

### Feature Files Not Found
```
Issue: "Could not find features..."
Solution: Make sure .feature files are in Features/ folder
and project file has <CompileFeatureFiles Include="Features/*.feature" />
```

## 📚 Step Definition Reference

### Common Given Steps
```csharp
[Given("I have obtained a valid region ID from the GetAll endpoint")]
[Given("I have retrieved all regions")]
[Given("I have a new region to create with:")]
```

### Common When Steps
```csharp
[When("I call the GetAll regions endpoint")]
[When("I call the GetRegion by ID endpoint with that ID")]
[When("I create the region")]
[When("I delete the region")]
```

### Common Then Steps
```csharp
[Then("the response status code should be 200 OK")]
[Then("the response should contain a list of regions")]
[Then("the response time should be less than (.*) seconds")]
```

## 🎯 Best Practices

1. **One Scenario = One Behavior**
   - Each scenario should test one specific behavior
   - Avoid testing multiple things in one scenario

2. **Use Meaningful Names**
   - Scenario names should be clear and descriptive
   - "Successfully retrieve all regions" ✅
   - "GetAll works" ❌

3. **Keep Steps Simple**
   - Each step should do one thing
   - Make steps reusable across scenarios
   - Avoid hardcoding values

4. **Use Data Tables for Complex Setup**
   ```gherkin
   Given I have regions:
       | Code | Name     |
       | AKL  | Auckland |
       | WLG  | Wellington |
   ```

5. **Group Related Scenarios**
   - Keep related scenarios in the same feature file
   - One feature file per API endpoint or feature

## 🔗 Quick Links

- [Reqnroll Documentation](https://reqnroll.net/)
- [Gherkin Guide](https://cucumber.io/docs/gherkin/)
- [Full BDD Guide](BDD_GUIDE.md)
- [Conversion Summary](BDD_CONVERSION_COMPLETE.md)

## 💡 Tips

- Use `dotnet test --list-tests` to see all available tests
- Tests are discovered automatically from .feature files
- Step definitions are matched by regex patterns
- Use `--filter` for quick test runs during development
- Run full test suite before committing code

## 📞 Need Help?

- Check `BDD_GUIDE.md` for comprehensive documentation
- See `StepDefinitions/RegionsStepDefinitions.cs` for implementation examples
- Review feature files for usage patterns

---

**Happy Testing! 🚀**
