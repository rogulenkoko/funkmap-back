using System;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Tests.Data;
using Funkmap.Common.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Module.Auth.Abstract;
using Funkmap.Module.Auth.Models;
using Funkmap.Module.Auth.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace Funkmap.Auth.Tests
{
    [TestClass]
    public class RegistrationTest
    {
        private IRegistrationContextManager _contextManager;
        private IAuthRepository _authRepository;

        [TestInitialize]
        public void Initialize()
        {

            var database = AuthTestDbProvider.DropAndCreateDatabase;
            var collection = database.GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName);

            var fileStorage = new Mock<IFileStorage>().Object;
            _authRepository = new AuthRepository(collection, fileStorage);

            var notificationService = new FakeExternalNotificationService();

            var logger = new Mock<ILogger>().Object;
            var funkmapLogger = new FunkmapLogger<RegistrationContextManager>(logger);

            _contextManager = new RegistrationContextManager(_authRepository, notificationService, new InMemoryStorage(), new FakeCodeGenerator(), funkmapLogger);

        }

        [TestMethod]
        public void RegisterPositive()
        {
            var request = new RegistrationRequest()
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                Locale = "ru",
                Password = Guid.NewGuid().ToString()
            };

            var existingUser = _authRepository.GetAsync(request.Login).GetAwaiter().GetResult();
            Assert.IsNull(existingUser);

            var result = _contextManager.TryCreateContextAsync(request).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);

            result = _contextManager.TryCreateContextAsync(request).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);

            var confirmationResult = _contextManager.TryConfirmAsync(request.Login, request.Email, $"{FakeCodeGenerator.Code}_1").GetAwaiter().GetResult();
            Assert.IsNotNull(confirmationResult);
            Assert.IsFalse(confirmationResult.Success);

            confirmationResult = _contextManager.TryConfirmAsync(request.Login, request.Email, FakeCodeGenerator.Code).GetAwaiter().GetResult();
            Assert.IsNotNull(confirmationResult);
            Assert.IsTrue(confirmationResult.Success);

            existingUser = _authRepository.GetAsync(request.Login).GetAwaiter().GetResult();

            Assert.IsNotNull(existingUser);
            Assert.AreEqual(existingUser.Login, request.Login);
            Assert.AreEqual(existingUser.Email, request.Email);
        }
    }

    public class FakeExternalNotificationService : IExternalNotificationService
    {
        public async Task<bool> TrySendNotificationAsync(Notification notification, NotificationOptions options = null)
        {
            await Task.Yield();
            return true;
        }
    }

    public class FakeCodeGenerator : IConfirmationCodeGenerator
    {
        public static string Code = "123123";
        public string Generate()
        {
            return Code;
        }
    }
}
