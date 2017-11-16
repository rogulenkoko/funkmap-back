using System.Collections.Generic;
using Funkmap.Statistics.Data.Objects;

namespace Funkmap.Statistics.Data.Services
{
    public interface IAgeInfoProvider
    {
        ICollection<AgeInfo> AgeInfos { get; }
    }

    public class AgeInfoProvider:IAgeInfoProvider
    {
        public ICollection<AgeInfo> AgeInfos => new List<AgeInfo>()
        {
            new AgeInfo(0,18),
            new AgeInfo(18,35),
            new AgeInfo(35,120)
        };
    }

    
}
