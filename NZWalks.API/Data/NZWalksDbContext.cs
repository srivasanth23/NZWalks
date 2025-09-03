using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        protected NZWalksDbContext()
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed data for difficulties
            // Easy, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("a1dd800e-b3b7-47a8-ae92-fd211eb90440"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("a28fc8d7-f932-4e50-8037-c35f25eb6a3e"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("32a9b788-58c8-491f-9804-719b823119dc"),
                    Name = "Hard"
                },
            };

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            // Seed data for regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("e0703812-0982-4822-8fd7-f18d2cd9a1ea"),
                    Name = "Vadodara",
                    Code = "BRC",
                    RegionImageUrl = "https://images.unsplash.com/photo-1688024098771-0722cbf34708?q=80&w=984&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                },
                new Region
                {
                    Id = Guid.Parse("a345a8b2-5d41-436e-a9a3-d8533281a40e"),
                    Name = "Ahemdabad",
                    Code = "AMD",
                    RegionImageUrl = "https://images.unsplash.com/photo-1651408451633-ff492f347ec1?w=1000&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8YWhtZWRhYmFkfGVufDB8fDB8fHww"
                },
                new Region
                {
                    Id = Guid.Parse("7a4c6a8a-d371-4615-8e48-b4a18fa1bbd6"),
                    Name = "Berhampur",
                    Code = "BAM",
                    RegionImageUrl = "https://images.unsplash.com/photo-1700332405166-ee87d4077591?w=1000&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8QmVyaGFtcHVyfGVufDB8fDB8fHww"
                },
                new Region
                {
                    Id = Guid.Parse("34f5471c-35f3-4d9e-bf69-a1437cb78f51"),
                    Name = "Bengaluru",
                    Code = "BANG",
                    RegionImageUrl = "https://images.unsplash.com/photo-1588416936097-41850ab3d86d?w=1000&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8YmVuZ2FsdXJ1fGVufDB8fDB8fHww"
                },
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            // Seed regions to the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
