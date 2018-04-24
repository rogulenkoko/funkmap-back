using System.Collections.Generic;
using Funkmap.Domain;
using Funkmap.Domain.Enums;

namespace Funkmap.Models.Requests
{
    public class FilteredRequest
    {
        public FilteredRequest()
        {
            Skip = 0;
            Take = 10;
        }

        /// <summary>
        /// Number of skipped elements
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Number of taken elements (default: 10)
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Search pattern (for names)
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Profile's creator login
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Profile type
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Interested musician instruments
        /// </summary>
        public List<Instruments> Instruments { get; set; }

        /// <summary>
        /// Interested musician expiriences
        /// </summary>
        public List<Expiriences> Expirience { get; set; }

        /// <summary>
        /// Interested musician styles (genres)
        /// </summary>
        public List<Styles> Styles { get; set; }

        /// <summary>
        /// Search within the radius (radians)
        /// </summary>
        public double? RadiusDeg { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Maximum number of profiles for reading (default: 1000)
        /// </summary>
        public int Limit { get; set; }

    }
}
