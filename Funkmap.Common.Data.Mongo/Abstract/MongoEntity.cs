using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Common.Data.Mongo.Abstract
{
    public abstract class MongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
