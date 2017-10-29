
using System;

namespace Funkmap.Common.Redis.Abstract
{
    public interface ISerializer
    {
        string Serialize(object value);

        T Deserialize<T>(string value) where T : class;
    }
}
