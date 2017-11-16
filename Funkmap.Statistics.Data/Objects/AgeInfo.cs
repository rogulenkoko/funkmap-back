
namespace Funkmap.Statistics.Data.Objects
{
    public class AgeInfo
    {
        public AgeInfo(int start, int finish)
        {
            Finish = finish;
            Start = start;
        }
        public int Start { get; }
        public int Finish { get; }

        public string AgePeriod => $"От {Start} до {Finish} лет";
    }
}
