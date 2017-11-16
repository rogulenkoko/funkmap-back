using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Statistics.Data.Objects;

namespace Funkmap.Statistics.Data.Services
{
    public class AgeInfoProvider:IAgeInfoProvider
    {
        public ICollection<AgeInfo> AgeInfos => new List<AgeInfo>()
        {
            new AgeInfo(0,10),
            new AgeInfo(10,19),
            new AgeInfo(19,28),
            new AgeInfo(28,37),
            new AgeInfo(37,46),
            new AgeInfo(46,100)
        };
    }

    public interface IAgeInfoProvider
    {
        ICollection<AgeInfo> AgeInfos { get; }
    }
}
