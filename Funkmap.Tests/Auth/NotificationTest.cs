using Autofac.Extras.Moq;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Settings;
using Funkmap.Middleware.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Auth
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void SendEmailTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide<ISettingsService>(new MonolithSettingsService());
                var service = mock.Create<EmailNotificationService>();
                var message = new ConfirmationNotification()
                {
                    Subject = "Test",
                    Body = "test",
                    Receiver = "rogulenkoko@gmail.com"
                };
                var success = service.SendNotification(message).Result;
                Assert.IsTrue(success);
            }
            
        }
    }

    public class ConfirmationNotification : Notification
    {
        
    }
}
