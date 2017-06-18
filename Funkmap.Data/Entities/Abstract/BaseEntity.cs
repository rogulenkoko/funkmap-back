using Funkmap.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Entities.Abstract
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("log")]
        public string Login { get; set; }

        [BsonElement("user")]
        public long UserId { get; set; } //todo изменить на ObjectId после переноса модуля авторизации на монгу

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("t")]
        public EntityType EntityType { get; set; }

        [BsonElement("loc")]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        [BsonElement("p")]
        [BsonIgnoreIfDefault]
        public BsonBinaryData Photo { get; set; }

        [BsonElement("d")]
        [BsonIgnoreIfDefault]
        public string Description { get; set; }

        [BsonElement("vk")]
        [BsonIgnoreIfDefault]
        public string VkLink { get; set; }

        [BsonElement("yt")]
        [BsonIgnoreIfDefault]
        public string YouTubeLink { get; set; }

        [BsonElement("fb")]
        [BsonIgnoreIfDefault]
        public string FacebookLink { get; set; }


    }
}
