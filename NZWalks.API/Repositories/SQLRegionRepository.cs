namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository//definition of class and implementation of interface
    {
        private readonly NZWalksDbContext dbContext;
        public SQLRegionRepository(NZWalksDbContext dbContext) => this.dbContext = dbContext;


        public async Task<Region> CreateAsync(Region region)
        {
            //region.Id = Guid.NewGuid();
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
          return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); 
        }

        public async Task<Region?>? UpdateAsync(Guid id, Region region)
        {
            var existingRegion = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            dbContext.Regions.Update(existingRegion);
            dbContext.SaveChanges();
            return existingRegion;
        }
    }
}