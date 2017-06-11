namespace Funkmap.Common.Data.Parameters
{
    public class SearchParameter
    {
        public LocationParameter LocationParameter { get; set; }
    }

    public class LocationParameter
    {
        public double RadiusDeg { get; set; }

        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        public double Longitude { get; set; }
    }
}
