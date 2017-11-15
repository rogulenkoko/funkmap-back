
namespace Funkmap.Statistics.Data.Entities
{
    public class InBandStatisticsEntity : BaseStatisticsEntity
    {
        public CountStatisticsEntity<bool> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            //todo
            return second;
        }
    }
}
