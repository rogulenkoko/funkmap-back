using System;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Contracts.Services;
using Funkmap.Common.RedisMq;
using ServiceStack.Messaging;

namespace Funkmap.Messenger.Services
{
    public class UserService : RedisMqProducer, IUserMqService
    {

        public UserService(IMessageFactory redisMqFactory) : base(redisMqFactory)
        {

        }

        public UserUpdateLastVisitDateResponse UpdateLastVisitDate(UserUpdateLastVisitDateRequest request)
        {
            var response = GetResponse<UserUpdateLastVisitDateResponse, UserUpdateLastVisitDateRequest>(request);
            return response;
        }
    }
}
