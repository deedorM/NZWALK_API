//namespace NZWalks.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegionsHardCodedController : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var regions = new List<Region>
//            {
//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Auckland Region",
//                    Code = "AKL",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1507699622108-4be3abd695ad"
//                },

//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Wellington Region",
//                    Code = "WLG",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1589871973318-9ca1258faa5d"
//                },

//                new Region
//                            {
//                Id = Guid.NewGuid(),
//                Name = "Canterbury Region",
//                Code = "CAN",
//                RegionImageUrl = "https://images.unsplash.com/photo-1469854523086-cc02fe5d8800"
//                },

//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Otago Region",
//                    Code = "OTA",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb"
//                },

//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Waikato Region",
//                    Code = "WKO",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1470770841072-f978cf4d019e"
//                },

//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Bay of Plenty Region",
//                    Code = "BOP",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee"
//                },

//                new Region
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Northland Region",
//                    Code = "NTL",
//                    RegionImageUrl = "https://images.unsplash.com/photo-1493246507139-91e8fad9978e"
//                }
//                            };

//            return Ok(regions);
//        }
//    }
//}