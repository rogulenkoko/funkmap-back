using Funkmap.Common;

namespace Funkmap.Data.Parameters
{
    public class CommonFilterParameter
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchText { get; set; }
        public EntityType EntityType { get; set; }
        public string UserLogin { get; set; }
    }
}
