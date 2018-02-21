using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Models.Requests
{
    public class FilteredRequest
    {
        /// <summary>
        /// Пропустить Skip элементов
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Взять Take элементов
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Часть имени
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Логин пользователя, создавшего профиль
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Тип профиля
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Интересующие инструменты
        /// </summary>
        public List<InstrumentType> Instruments { get; set; }

        /// <summary>
        /// Интересующие типы опыта игры
        /// </summary>
        public List<ExpirienceType> Expirience { get; set; }

        /// <summary>
        /// Интересующие стили игры
        /// </summary>
        public List<Styles> Styles { get; set; }

        /// <summary>
        /// Поиск внутри радиуса (в радианах)
        /// </summary>
        public double? RadiusDeg { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Максимальное количество доступных для чтение сущностей
        /// </summary>
        public int Limit { get; set; }

    }
}
