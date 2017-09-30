using Funkmap.Common;

namespace Funkmap.Data.Parameters
{
    public class CommonFilterParameter : LocationParameter
    {
        public string SearchText { get; set; }
        public EntityType EntityType { get; set; }
        public string UserLogin { get; set; }
        
        /// <summary>
        /// Максимальное количество доступных для чтение сущностей
        /// </summary>
        public int Limit { get; set; }
    }
}
