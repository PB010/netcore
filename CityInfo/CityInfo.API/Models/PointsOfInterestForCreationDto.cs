using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestForCreationDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description should have a max length of 200 characters.")]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
