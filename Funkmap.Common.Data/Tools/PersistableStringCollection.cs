
using System;
using Funkmap.Common.Data.Abstract;

namespace Funkmap.Common.Data.Tools
{
    public class PersistableStringCollection: PersistableCollection<string>
    {
        protected override string ConvertSingleValueToRuntime(string rawValue)
        {
            return rawValue;
        }

        protected override string ConvertSingleValueToPersistable(string value)
        {
            return value;
        }
    }
}
