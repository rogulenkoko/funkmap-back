using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Notifications.Data.Entities
{
    public class NotificationEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nt")]
        public string NotificationType { get; set; }

        [BsonElement("nd")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("nr")]
        public bool IsRead { get; set; }

        [BsonElement("nrl")]
        public string ReceiverLogin { get; set; }

        [BsonElement("nsl")]
        public string SenderLogin { get; set; }

        [BsonElement("na")]
        public bool NeedAnswer { get; set; }

        [BsonElement("inn")]
        public string InnerNotificationJson { get; set; }
    }
}
