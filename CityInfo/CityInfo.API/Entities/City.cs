﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public ICollection<PointsOfInterest> PointsOfInterest { get; set; }
            = new List<PointsOfInterest>();
    }
}
