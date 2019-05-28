using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("/api/cities/")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointsOfInterest = _cityInfoRepository.GetPointsOfInterests(cityId)
                .ToList().Select(Mapper.Map<PointsOfInterest, PointsOfInterestDto>);

            return Ok(pointsOfInterest);
        
        }

        [HttpGet("{cityId}/pointsOfInterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterest(cityId, id);
            
            if (pointOfInterest == null)
                return NotFound();

            return Ok(Mapper.Map<PointsOfInterestDto>(pointOfInterest));
        }

        [HttpPost("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointsOfInterestForCreationDto pointOfInterest)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from name.");

            if (!ModelState.IsValid)
                return BadRequest();

            var result = Mapper.Map<PointsOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, result);

            if (!_cityInfoRepository.Save())
                return StatusCode(500, "A problem happened with your request.");

            var createdPointOfInterest = Mapper.Map<PointsOfInterestDto>(result);

            return CreatedAtRoute("GetPointOfInterest",
                new {cityId, id = createdPointOfInterest.Id},
                createdPointOfInterest);
        }

        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] UpdatePointOfInterestDto pointOfInterest)
        {
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from name.");

            if (!ModelState.IsValid)
                return BadRequest();

            var pointOfInterestFromDb = _cityInfoRepository.GetPointOfInterest(cityId, id);

            if (pointOfInterestFromDb == null)
                return NotFound();

            Mapper.Map(pointOfInterest, pointOfInterestFromDb);

            if (!_cityInfoRepository.Save())
                return StatusCode(500, "An error");

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        public IActionResult PartiallyUpdatedPointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<UpdatePointOfInterestDto> patchDoc)
        {
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            if (patchDoc == null)
                return BadRequest();

            var pointOfInterestFromDb = _cityInfoRepository.GetPointOfInterest(cityId, id);
            if (pointOfInterestFromDb == null)
                return NotFound();

            var pointOfInterestToPatch = Mapper.Map<UpdatePointOfInterestDto>(pointOfInterestFromDb);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
                ModelState.AddModelError("Description", "Name and description cannot be the same.");

            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
                return BadRequest();

            Mapper.Map(pointOfInterestToPatch, pointOfInterestFromDb);

            if (!_cityInfoRepository.Save())
                return StatusCode(500, "A problem happened with your request.");

            return NoContent();

        }

        [HttpDelete("{cityId}/pointsOfInterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var pointOfInterestFromDb = _cityInfoRepository.GetPointOfInterest(cityId, id);
            if (pointOfInterestFromDb == null)
                return NotFound();

            _cityInfoRepository.RemovePointOfInterestFromCity(pointOfInterestFromDb);

            if (!_cityInfoRepository.Save())
                return StatusCode(500, "A problem happened with your request.");


            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestFromDb.Name} with id" +
                $"{pointOfInterestFromDb.Id} was deleted.");

            return NoContent();
        }
    }
}
