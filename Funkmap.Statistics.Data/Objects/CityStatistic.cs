using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class CityStatistic
    {

        [BsonId]
        [BsonElement("key")]
        public string NameCity { get; set; }

        [BsonElement("value")]
        public Object Count { get; set; }
    }
}
