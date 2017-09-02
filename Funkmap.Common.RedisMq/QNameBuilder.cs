using System;

namespace Funkmap.Auth.Contracts
{
    public class QNameBuilder
    {
        public static string BuildQueueName()
        {
            return $"mq:funkmap:{Guid.NewGuid().ToString("N")}";
        }
       
    }
}
