namespace Funkmap.Domain.Parameters
{
    public class LocationParameter
    {
        public double? RadiusDeg { get; set; }

        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double? Longitude { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
