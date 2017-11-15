using System.Collections.Generic;
using Funkmap.Statistics.Data.Objects;

namespace Funkmap.Statistics.Data.Services
{
    public interface ICitiesInfoProvider
    {
        ICollection<CityInfo> CityInfos { get; }
    }

    public class CitiesInfoProvider : ICitiesInfoProvider
    {

        public ICollection<CityInfo> CityInfos => new List<CityInfo>()
        {
            new CityInfo() {Name = "Санкт-Петербург", CenterLatitude = 50, CenterLongitude = 30, Radius = 2},
            new CityInfo() { Name = "Минск", CenterLatitude = 55, CenterLongitude = 29, Radius = 2}
        };
    }
}
