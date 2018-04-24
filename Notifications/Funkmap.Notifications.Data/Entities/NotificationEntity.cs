using System;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Notifications.Data.Entities
{
    public class NotificationEntity
    {

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nt")]
        public NotificationType NotificationType { get; set; }

        [BsonElement("nd")]
        public DateTime Date { get; set; }

        [BsonElement("nr")]
        public bool IsRead { get; set; }

        [BsonElement("nrl")]
        public string RecieverLogin { get; set; }

        [BsonElement("nsl")]
        public string SenderLogin { get; set; }

        [BsonElement("na")]
        public bool NeedAnswer { get; set; }

        [BsonElement("inn")]
        public NotificationBase InnerNotification { get; set; }
    }
}
