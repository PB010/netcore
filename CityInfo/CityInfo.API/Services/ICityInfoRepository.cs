using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        bool CityExists(int cityId);
        City GetCity(int cityId, bool pointsOfInterest);
        IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId);
        PointsOfInterest GetPointOfInterest(int cityId, int pointOfInterestId);
        bool Save();
        void RemovePointOfInterestFromCity(PointsOfInterest pointsOfInterest);
        void AddPointOfInterestForCity(int cityId, PointsOfInterest pointsOfInterest);

    }
}
