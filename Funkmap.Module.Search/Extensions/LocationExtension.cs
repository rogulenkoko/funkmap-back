using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract.Search;

namespace Funkmap.Module.Search.Extensions
{
    public static class LocationExtension
    {
        public static ICollection<SearchModel> SortByLocationToPoint(this ICollection<SearchModel> models, double pointLon, double pointLat)
        {
            Dictionary<SearchModel, double> dictionary = models.ToDictionary(x => x, y => Math.Abs(y.Longitude - pointLon) + Math.Abs(y.Latitude - pointLat));
            var orderedValues = dictionary.OrderBy(x=>x.Value).Select(x=>x.Key).ToList();
            return orderedValues;
        }
    }
}
