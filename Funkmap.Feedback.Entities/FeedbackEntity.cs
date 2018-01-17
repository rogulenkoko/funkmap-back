using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Feedback.Entities
{
    public class FeedbackEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("t")]
        public FeedbackType FeedbackType { get; set; }

        [BsonElement("m")]
        public string Message { get; set; }
    }

    //todo вынести это в сборку Funkmap.Feedback.Entities
    public enum FeedbackType
    {
        Another = 0,
        Bug = 1,
        Feature = 2
    }
}
