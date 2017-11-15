using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class CityStatistic
    {

        [BsonId]
        public string City { get; set; }

        [BsonElement("value")]
        public int Count { get; set; }
    }
}
