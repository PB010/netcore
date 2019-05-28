using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using CityInfo.API.Entities;

namespace CityInfo.API.Controllers
{
    [Route("/api/cities/")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _cityInfoRepository
                .GetCities()
                .Select(Mapper.Map<City, CitiesWithoutPointsOfInterestDto>);

            return Ok(cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {
            var cityToReturn = _cityInfoRepository.GetCity(id, includePointOfInterest);

            if (cityToReturn == null)
                return NotFound();

            if (includePointOfInterest)
            {
                var result = Mapper.Map<CityDto>(cityToReturn);
                return Ok(result);
            }

            var resultNoPoints = Mapper.Map<CitiesWithoutPointsOfInterestDto>(cityToReturn);
            return Ok(resultNoPoints);
        }
    }
}
