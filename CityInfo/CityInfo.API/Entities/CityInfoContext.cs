using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
        : base(options)
        {
            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointsOfInterest> PointsOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park"
                },
                new City
                {
                    Id = 2,
                    Name = "Paris",
                    Description = "The one with that big tower."
                }
            );

            modelBuilder.Entity<PointsOfInterest>().HasData(
                new PointsOfInterest
                {
                    Id = 1,
                    CityId = 1,
                    Name = "Central Park",
                    Description = "The most visited urban park."
                },
                new PointsOfInterest
                {
                    Id = 2,
                    CityId = 1,
                    Name = "Empire State Building",
                    Description = "A 102 story skyscraper."
                }, new PointsOfInterest
                {
                    Id = 3,
                    CityId = 2,
                    Name = "Eiffel Tower",
                    Description = "A wrought iron lattice tower."
                },
                new PointsOfInterest
                {
                    Id = 4,
                    CityId = 2,
                    Name = "The Louvre",
                    Description = "The world's largest museum."
                }
            );

            base.OnModelCreating(modelBuilder);
            

        }
    }
}
