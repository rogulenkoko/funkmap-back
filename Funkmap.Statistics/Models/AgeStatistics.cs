namespace Funkmap.Statistics.Models
{
    public class AgeStatistics : IStatistics
    {
        public string Type { get; set; }

        public int Count { get; set; }
    }

    public enum AgeType
    {
        None = 0,
        Less18 = 1,
        More18Less35 = 2,
        More35 = 3
    }
}
