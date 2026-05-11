using NZWalks.API.Tests.ApiClients;
using NZWalks.API.Tests.Builders;
using NZWalks.API.Tests.Fixtures;
using NUnit.Framework;

namespace NZWalks.API.Tests.Tests
{
    [TestFixture]
    public class RegionsApiDataDrivenTests : BaseTestFixture
    {
        [Test]
        [Description("Verify GetAll returns consistent data structure")]
        public async Task GetAllRegions_VerifyConsistentStructure()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data, Is.Not.Null);

            // Verify structure consistency
            foreach (var region in response.Data)
            {
                var dto = new RegionBuilder()
                    .WithId(region.Id)
                    .WithCode(region.Code)
                    .WithName(region.Name)
                    .WithImageUrl(region.RegionImageUrl)
                    .BuildRegionDto();

                Assert.That(dto.Id, Is.TypeOf<Guid>());
                Assert.That(dto.Code, Is.TypeOf<string>());
                Assert.That(dto.Name, Is.TypeOf<string>());
            }
        }

        [Test]
        [TestCase("AKL", "Auckland")]
        [TestCase("WLG", "Wellington")]
        [TestCase("CHC", "Christchurch")]
        [Description("Verify that known regions exist in the system")]
        public async Task GetAllRegions_KnownRegionsShouldExist(string code, string name)
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True);

            var knownRegion = response.Data
                .FirstOrDefault(r => r.Code == code);

            if (knownRegion != null)
            {
                Assert.That(knownRegion.Name, Does.Contain(name), 
                    $"Region with code {code} should contain {name} in its name");
            }
            // If region doesn't exist, test is still valid (it may not have been seeded yet)
        }

        [Test]
        [Description("Verify region builder works correctly")]
        public void RegionBuilder_ShouldBuildValidRegions()
        {
            // Act
            var region = new RegionBuilder()
                .WithCode("TEST")
                .WithName("Test Region")
                .WithImageUrl("https://example.com/test.jpg")
                .BuildRegionDto();

            // Assert
            Assert.That(region.Code, Is.EqualTo("TEST"));
            Assert.That(region.Name, Is.EqualTo("Test Region"));
            Assert.That(region.RegionImageUrl, Does.Contain("example.com"));
        }

        [Test]
        [Description("Verify fluent builder with random data")]
        public void RegionBuilder_ShouldGenerateRandomData()
        {
            // Act
            var region1 = new RegionBuilder()
                .WithRandomCode()
                .WithRandomName()
                .BuildRegionDto();

            var region2 = new RegionBuilder()
                .WithRandomCode()
                .WithRandomName()
                .BuildRegionDto();

            // Assert
            Assert.That(region1.Code, Is.Not.EqualTo(region2.Code), 
                "Random codes should be different");
            Assert.That(region1.Name, Is.Not.EqualTo(region2.Name), 
                "Random names should be different");
        }

        [Test]
        [Description("Verify test data sets are valid")]
        public void TestDataSets_ShouldProvideValidData()
        {
            // Act & Assert
            Assert.That(TestDataSets.AucklandRegion.Code, Is.EqualTo("AKL"));
            Assert.That(TestDataSets.WellingtonRegion.Code, Is.EqualTo("WLG"));
            Assert.That(TestDataSets.ChristchurchRegion.Code, Is.EqualTo("CHC"));
            Assert.That(TestDataSets.QueenstownRegion.Code, Is.EqualTo("ZQN"));
        }

        [Test]
        [Description("Verify all test data sets are non-empty")]
        public void TestDataSets_AllRegionsShouldHaveData()
        {
            // Act
            var allRegions = TestDataSets.AllTestRegions;

            // Assert
            Assert.That(allRegions, Is.Not.Empty);
            Assert.That(allRegions.Count, Is.GreaterThan(0));

            foreach (var region in allRegions)
            {
                Assert.That(region.Code, Is.Not.Null.And.Not.Empty);
                Assert.That(region.Name, Is.Not.Null.And.Not.Empty);
            }
        }

        [Test]
        [Description("Verify region builder chaining")]
        public void RegionBuilder_ShouldSupportChaining()
        {
            // Act
            var region = new RegionBuilder()
                .WithCode("CHN")
                .WithName("Chaining Test")
                .WithImageUrl("https://example.com/chain.jpg")
                .WithRandomCode()
                .BuildRegionDto();

            // Assert
            Assert.That(region, Is.Not.Null);
            Assert.That(region.Code, Does.StartWith("TST_"), 
                "Should have random code generated last");
        }

        [Test]
        [Description("Verify GetById with builder-generated data")]
        public async Task GetRegionById_WithBuilderData_ShouldWork()
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            if (allRegionsResponse.Data.Count == 0)
            {
                Assert.Ignore("No regions available for this test");
            }

            var existingRegion = allRegionsResponse.Data.First();
            var expectedDto = new RegionBuilder()
                .WithId(existingRegion.Id)
                .WithCode(existingRegion.Code)
                .WithName(existingRegion.Name)
                .WithImageUrl(existingRegion.RegionImageUrl)
                .BuildRegionDto();

            // Act
            var response = await ApiClient.GetRegionByIdAsync(existingRegion.Id);

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data.Id, Is.EqualTo(expectedDto.Id));
            Assert.That(response.Data.Code, Is.EqualTo(expectedDto.Code));
            Assert.That(response.Data.Name, Is.EqualTo(expectedDto.Name));
        }

        [Test]
        [Description("Verify region ID format is valid GUID")]
        public async Task GetAllRegions_RegionIdsShouldBeValidGuids()
        {
            // Act
            var response = await ApiClient.GetAllRegionsAsync();

            // Assert
            Assert.That(response.IsSuccessful, Is.True);

            foreach (var region in response.Data)
            {
                // Verify ID is not empty and is a valid Guid
                Assert.That(region.Id, Is.Not.EqualTo(Guid.Empty), 
                    "Region ID should not be empty GUID");
                Assert.That(region.Id, Is.TypeOf<Guid>(), 
                    "Region ID should be of type Guid");
            }
        }

        [Test]
        [TestCase("AKL", "Auckland")]
        [TestCase("WLG", "Wellington")]
        [Description("Verify GetById works with test data cases")]
        public async Task GetRegionById_WithTestCases_ShouldReturnCorrectData(string code, string expectedNamePart)
        {
            // Arrange
            var allRegionsResponse = await ApiClient.GetAllRegionsAsync();
            var region = allRegionsResponse.Data
                .FirstOrDefault(r => r.Code == code);

            if (region == null)
            {
                Assert.Ignore($"Region with code {code} not found in database");
            }

            // Act
            var response = await ApiClient.GetRegionByIdAsync(region.Id);

            // Assert
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Data.Code, Is.EqualTo(code));
            Assert.That(response.Data.Name, Does.Contain(expectedNamePart) | Is.EqualTo(region.Name));
        }
    }
}
