using System;
using System.Linq;
using Autofac;
using Funkmap.Module;
using Funkmap.Notifications;
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

            var funkmapModule = new FunkmapModule();
            funkmapModule.Register(builder);

            var notificationModule = new NotificationsModule();
            notificationModule.Register(builder);

            _container = builder.Build();

           

        }

        [TestMethod]
        public void InviteBandTest()
        {
            var specificNotificationService = _container.Resolve<FunkmapNotificationService>();

            //todo
            //var baseNotificationService = _container.Resolve<IEnumerable<INotificationsService>>().FirstOrDefault();

            //var request = new InviteToBandRequest()
            //{
            //    InvitedMusicianLogin = "test",
            //    SenderLogin = "rogulenkoko",
            //    BandLogin = "beatles",
                
            //};
            //specificNotificationService.NotifyBandInvite(request);
            //todo
            //baseNotificationService.PublishBackRequest(new InviteToBandBack() {Notification = request, Answer = true});
        }
    }
}
