﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepo
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetById(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region> UpdateAsync(Guid id, Region region);
        Task<Region> DeleteAsync(Guid id);
    }
}
