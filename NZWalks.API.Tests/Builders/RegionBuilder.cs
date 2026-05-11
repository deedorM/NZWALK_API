namespace NZWalks.API.Tests.Builders
{
    /// <summary>
    /// Builder pattern for creating test regions with fluent API
    /// </summary>
    public class RegionBuilder
    {
        private string _code = "TST";
        private string _name = "Test Region";
        private string _imageUrl = "https://example.com/image.jpg";
        private Guid _id = Guid.Empty;

        public RegionBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public RegionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public RegionBuilder WithImageUrl(string url)
        {
            _imageUrl = url;
            return this;
        }

        public RegionBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public RegionBuilder WithRandomCode()
        {
            _code = $"TST_{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";
            return this;
        }

        public RegionBuilder WithRandomName()
        {
            _name = $"Region_{Guid.NewGuid().ToString().Substring(0, 8)}";
            return this;
        }

        public CreateRegionRequest BuildCreateRequest()
        {
            return new CreateRegionRequest
            {
                Code = _code,
                Name = _name,
                RegionImageUrl = _imageUrl
            };
        }

        public RegionDto BuildRegionDto()
        {
            return new RegionDto
            {
                Id = _id == Guid.Empty ? Guid.NewGuid() : _id,
                Code = _code,
                Name = _name,
                RegionImageUrl = _imageUrl
            };
        }
    }

    /// <summary>
    /// Pre-built test data sets
    /// </summary>
    public static class TestDataSets
    {
        public static CreateRegionRequest AucklandRegion =>
            new RegionBuilder()
                .WithCode("AKL")
                .WithName("Auckland")
                .WithImageUrl("https://example.com/auckland.jpg")
                .BuildCreateRequest();

        public static CreateRegionRequest WellingtonRegion =>
            new RegionBuilder()
                .WithCode("WLG")
                .WithName("Wellington")
                .WithImageUrl("https://example.com/wellington.jpg")
                .BuildCreateRequest();

        public static CreateRegionRequest ChristchurchRegion =>
            new RegionBuilder()
                .WithCode("CHC")
                .WithName("Christchurch")
                .WithImageUrl("https://example.com/christchurch.jpg")
                .BuildCreateRequest();

        public static CreateRegionRequest QueenstownRegion =>
            new RegionBuilder()
                .WithCode("ZQN")
                .WithName("Queenstown")
                .WithImageUrl("https://example.com/queenstown.jpg")
                .BuildCreateRequest();

        public static List<CreateRegionRequest> AllTestRegions =>
            new List<CreateRegionRequest>
            {
                AucklandRegion,
                WellingtonRegion,
                ChristchurchRegion,
                QueenstownRegion
            };

        public static CreateRegionRequest RegionWithSpecialCharacters =>
            new RegionBuilder()
                .WithCode("SPC")
                .WithName("Region with Special Chars: @#$%^&*()")
                .WithImageUrl("https://example.com/special.jpg?param=value&other=123")
                .BuildCreateRequest();

        public static CreateRegionRequest RegionWithLongName =>
            new RegionBuilder()
                .WithCode("LNG")
                .WithName("A Very Long Region Name " + new string('X', 100))
                .WithImageUrl("https://example.com/long.jpg")
                .BuildCreateRequest();

        public static CreateRegionRequest RegionWithMinimalData =>
            new RegionBuilder()
                .WithCode("MIN")
                .WithName("M")
                .WithImageUrl("")
                .BuildCreateRequest();
    }
}
