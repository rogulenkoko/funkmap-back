using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Messenger.Services;
using Funkmap.Module.Auth;
using Funkmap.Module.Auth.Services;
using Funkmap.Tests.Auth.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class UserMqServiceTest
    {
        private UserService _userService;

        [TestInitialize]
        public void Initialize()
        {
            var redisHost = "localhost:6379";
            IRedisClientsManager redisClientManager = new PooledRedisClientManager(redisHost);

            IMessageFactory redisMqFactory = new RedisMessageFactory(redisClientManager);

            IMessageService redisMqServer = new RedisMqServer(redisClientManager, retryCount: 2);
            

            var repository = new AuthRepository(AuthTestDbProvider.DropAndCreateDatabase.GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName));

            UserMqService baseMqService = new UserMqService(redisMqServer, repository);
            baseMqService.InitHandlers();
            _userService = new UserService(redisMqFactory);
            redisMqServer.Start();

        }
        
        [TestMethod]
        public void GetLastVisitDate()
        {

            var updateRequest = new UserUpdateLastVisitDateRequest()
            {
                Login = "rogulenkoko"
            };

            var updateResult = _userService.UpdateLastVisitDate(updateRequest);
            Assert.IsNotNull(updateResult);
            Assert.IsTrue(updateResult.Success);
        }
    }
}
