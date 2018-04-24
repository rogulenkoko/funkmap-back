namespace Funkmap.Domain.Abstract
{
    public interface ILocationParameter
    {
        /// <summary>
        /// Радиус в километрах
        /// </summary>
        double? RadiusKm { get; set; }

        /// <summary>
        /// Широта объекта относительно которого надо искать
        /// </summary>
        double? Latitude { get; set; }

        /// <summary>
        /// Долгота объекта относительно которого надо искать
        /// </summary>
        double? Longitude { get; set; }

    }
}
