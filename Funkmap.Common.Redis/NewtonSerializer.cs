using System;
using Funkmap.Common.Redis.Abstract;
using Newtonsoft.Json;

namespace Funkmap.Common.Redis
{
    public class NewtonSerializer : ISerializer
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value) where T : class 
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            
        }
    }
}
