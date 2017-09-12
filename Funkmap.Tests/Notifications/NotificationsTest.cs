using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autofac;
using Funkmap.Common.Modules;
using Funkmap.Contracts.Notifications;
using Funkmap.Module;
using Funkmap.Notifications;
using Funkmap.Notifications.Services.Abstract;
using Funkmap.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Notifications
{
    [TestClass]
    public class NotificationsTest
    {
        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            AppDomain.CurrentDomain.GetAssemblies().Select(x => AppDomain.CurrentDomain.Load(x.GetName()));
            var builder = new ContainerBuilder();
            var redisMqModule = new RedisMqModule();
            redisMqModule.Register(builder);

            var funkmapModule = new FunkmapModule();
            funkmapModule.Register(builder);

            builder.RegisterType<FunkmapNotificationService>();

            var notificationModule = new NotificationsModule();
            notificationModule.Register(builder);

            _container = builder.Build();

           

        }

        [TestMethod]
        public void InviteBandTest()
        {
            var specificNotificationService = _container.Resolve<FunkmapNotificationService>();

            var baseNotificationService = _container.Resolve<IEnumerable<INotificationsService>>().FirstOrDefault();

            var request = new InviteToBandRequest()
            {
                InvitedMusicianLogin = "test",
                InviterLogin = "rogulenkoko",
                BandLogin = "beatles",
                
            };
            specificNotificationService.InviteMusicianToGroup(request);
            baseNotificationService.PublishBackRequest(new InviteToBandBack() {RequestId = request.Id, Answer = true});
        }
    }
}
