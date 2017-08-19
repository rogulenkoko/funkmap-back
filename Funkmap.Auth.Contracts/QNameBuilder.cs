using System;

namespace Funkmap.Auth.Contracts
{
    public class QNameBuilder
    {
        public static string BuildQueueName(string serviceName)
        {
            return $"mq:{serviceName}:{Guid.NewGuid().ToString("N")}";
        }
       
    }
}
