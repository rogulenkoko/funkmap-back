using System.Diagnostics;
using System.Threading;
using Autofac;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Modules;
using Funkmap.Messenger.Services;
using Funkmap.Module.Auth;
using Funkmap.Module.Auth.Services;
using Funkmap.Notifications.Contracts.Funkmap;
using Funkmap.Notifications.Services;
using Funkmap.Services;
using Funkmap.Tests.Auth.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Funkmap.Tests.Notifications
{
    [TestClass]
    public class NotificationsTest
    {
        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            var redisMqModule = new RedisMqModule();
            redisMqModule.Register(builder);
            builder.RegisterType<BandInviteNotificationsService>();
            builder.RegisterType<FunkmapNotificationService>();
            _container = builder.Build();

        }

        [TestMethod]
        public void InviteBandTest()
        {
            var specificNotificationService = _container.Resolve<FunkmapNotificationService>();

            var baseNotificationService = _container.Resolve<BandInviteNotificationsService>();

            var request = new InviteToGroupRequest()
            {
                InvitedMusicianLogin = "test",
                InviterLogin = "rogulenkoko",
                GroupLogin = "beatles",
                
            };
            specificNotificationService.InviteMusicianToGroup(request);
            Thread.Sleep(5000);
            baseNotificationService.PublishBackRequest(new InviteToGroupBackRequest() {RequestId = request.Id, Answer = true});
        }
    }
}
