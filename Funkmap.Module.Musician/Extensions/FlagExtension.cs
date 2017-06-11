using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Extensions
{
    public static class FlagExtension
    {
        public static InstrumentType[] ToArray(this InstrumentType flags)
        {
            return Enum.GetValues(typeof(InstrumentType)).Cast<InstrumentType>().Where(allStyles => (flags & allStyles) != 0).ToArray();
        }

        public static Styles[] ToArray(this Styles flags)
        {
            return Enum.GetValues(typeof(Styles)).Cast<Styles>().Where(allStyles => (flags & allStyles) != 0).ToArray();
        }
    }
}
