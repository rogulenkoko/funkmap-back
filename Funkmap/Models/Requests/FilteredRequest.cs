using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Models.Requests
{
    public class FilteredRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchText { get; set; }
        public EntityType EntityType { get; set; }
        public List<InstrumentType> Instruments { get; set; }
        public List<ExpirienceType> Expirience { get; set; }
        public List<Styles> Styles { get; set; }

        public double? RadiusDeg { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        /// <summary>
        /// Максимальное количество доступных для чтение сущностей
        /// </summary>
        public int Limit { get; set; }

    }
}
