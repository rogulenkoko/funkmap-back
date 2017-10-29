using System;
using Newtonsoft.Json.Serialization;

namespace Funkmap.Common.Redis.Options
{
    public class MessageQueueOptions
    {
        /// <summary>
        /// Дополнительная часть ключа (дополнению к типу сообщения)
        /// </summary>
        public object SpecificKey { get; set; }


        /// <summary>
        /// Тип в который надо сериализовать по возвращению
        /// </summary>
        public Type MessageType { get; set; }
    }
}
