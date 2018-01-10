using System;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Newtonsoft.Json;

namespace Funkmap.Common.Redis
{
    public class NewtonSerializer : ISerializer
    {
        public string Serialize(object value, SerializerOptions options = null)
        {
            var newtonOptions = new JsonSerializerSettings();

            if (options != null && options.HasAbstractMember)
            {
                newtonOptions.TypeNameHandling = TypeNameHandling.All;
            }

            return JsonConvert.SerializeObject(value, newtonOptions);
        }

        public T Deserialize<T>(string value, SerializerOptions options = null) where T : class 
        {
            try
            {
                var newtonOptions = new JsonSerializerSettings();

                if (options != null && options.HasAbstractMember)
                {
                    newtonOptions.TypeNameHandling = TypeNameHandling.All;
                }
                
                return JsonConvert.DeserializeObject<T>(value, newtonOptions);
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            
        }
    }
}
