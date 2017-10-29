using System;

namespace Funkmap.Common.Redis.Options
{
    public class MessageQueueOptions
    {
        /// <summary>
        /// Дополнительная часть ключа (дополнению к типу сообщения)
        /// </summary>
        public object SpecificKey { get; set; }

        /// <summary>
        /// Настройки сериализации
        /// </summary>
        public SerializerOptions SerializerOptions { get; set; }
    }
}
