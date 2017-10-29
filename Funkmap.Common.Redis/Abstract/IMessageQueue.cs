using System;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Options;

namespace Funkmap.Common.Redis.Abstract
{
    public interface IMessageQueue
    {
        /// <summary>
        /// Публикация уведомления в очередь с типо обьекта в качестве ключа
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task PublishAsync(object value, MessageQueueOptions options = null);


        /// <summary>
        /// Публикация уведомления в очередь с кастомным ключом
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task PublishAsync(string key, object value, MessageQueueOptions options = null);

        /// <summary>
        /// Подписка на событие, где ключоом выступает тип сообщения (плюс опциональный ключ из MessageQueueOptions)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">Обработчик события</param>
        /// <param name="options">Дополнительные опции</param>
        void Subscribe<T>(Action<T> handler, MessageQueueOptions options = null) where T : class;


        /// <summary>
        /// Подписка на событие с кастомным ключом
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Ключ подписки</param>
        /// <param name="handler">Обработчик события</param>
        /// <param name="options">Дополнительные опции</param>
        void Subscribe<T>(string key, Action<T> handler, MessageQueueOptions options = null) where T : class;
    }
}
