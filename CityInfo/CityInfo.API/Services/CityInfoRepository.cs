using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool pointsOfInterest)
        {
            if (pointsOfInterest)
            {
                return _context.Cities
                    .Include(c => c.PointsOfInterest)
                    .SingleOrDefault(c => c.Id == cityId);
            }

            return _context.Cities.SingleOrDefault(c => c.Id == cityId);
        }

        public IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId)
        {
            return _context.PointsOfInterests.Where(p => p.CityId == cityId).ToList();
        }

        public PointsOfInterest GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterests.SingleOrDefault(p => p.Id == pointOfInterestId && p.CityId == cityId);
        }

        public void RemovePointOfInterestFromCity(PointsOfInterest pointsOfInterest)
        {
            
            _context.PointsOfInterests.Remove(pointsOfInterest);
        }

        public void AddPointOfInterestForCity(int cityId, PointsOfInterest pointsOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointsOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
