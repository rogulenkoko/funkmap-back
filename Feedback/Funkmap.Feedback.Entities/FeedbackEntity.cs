using System;
using System.Collections.Generic;
using Funkmap.Feedback.Domain;
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

        [BsonElement("c")]
        public List<FeedbackContentEntity> Content { get; set; }
        
        [BsonElement("cr")]
        public DateTime Created { get; set; }
    }

    public class FeedbackContentEntity
    {
        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("url")]
        public string DataUrl { get; set; }
    }
}
