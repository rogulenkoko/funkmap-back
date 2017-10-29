using System;
using Funkmap.Common.Redis.Abstract;
using Newtonsoft.Json;

namespace Funkmap.Common.Redis
{
    public class NewtonSerializer : ISerializer
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public T Deserialize<T>(string value) where T : class 
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            
        }
    }
}
