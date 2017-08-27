using System.Collections.Generic;
using Funkmap.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Entities.Abstract
{
    [BsonDiscriminator(RootClass = true)]

    [BsonKnownTypes(
        typeof(MusicianEntity), 
        typeof(ShopEntity), 
        typeof(BandEntity),
        typeof(StudioEntity),
        typeof(RehearsalPointEntity))]
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("log")]
        public string Login { get; set; }

        [BsonElement("user")]
        public string UserLogin { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("t")]
        public EntityType EntityType { get; set; }

        [BsonElement("loc")]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        [BsonElement("a")]
        public string Address { get; set; }

        [BsonElement("p")]
        [BsonIgnoreIfDefault]
        public BsonBinaryData Photo { get; set; }

        [BsonElement("ytv")]
        public List<string> YouTubeVideoLins { get; set; }

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

        [BsonElement("sc")]
        [BsonIgnoreIfDefault]
        public string SoundCloudLink { get; set; }

        [BsonElement("gallery")]
        [BsonIgnoreIfDefault]
        public List<BsonBinaryData> Gallery { get; set; }

    }
}
