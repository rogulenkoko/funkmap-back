
using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class FullLocationRequest
    {
        [Required]
        public double? RadiusKm { get; set; }

        [Required]
        public double? Latitude { get; set; }

        [Required]
        public double? Longitude { get; set; }

        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }
    }
}
