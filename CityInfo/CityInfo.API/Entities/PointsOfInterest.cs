using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities
{
    public class PointsOfInterest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
