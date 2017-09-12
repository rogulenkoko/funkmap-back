using System;
using Funkmap.Notifications.Contracts;
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


        /// <summary>
        /// Сериализованное уведомление
        /// </summary>
        [BsonElement("inn")]
        public string InnerNotificationJson { get; set; }
    }
}
