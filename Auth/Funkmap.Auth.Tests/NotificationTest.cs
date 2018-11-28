using Autofac.Extras.Moq;
using Funkmap.Auth.Notifications;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Funkmap.Auth.Tests
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void SendConfirmationTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide<ISettingsService>(new FakeSettingsService());
                var service = mock.Create<EmailExternalNotificationService>();
                var message = new ConfirmationNotification("rogulenkoko@gmail.com", "Кирилл Рогуленко", "123321");
                var success = service.TrySendNotificationAsync(message).Result;
                Assert.IsTrue(success);
            }
        }

        [TestMethod]
        public void SendRecoveryPasswordTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide<ISettingsService>(new FakeSettingsService());
                var service = mock.Create<EmailExternalNotificationService>();
                var message = new PasswordRecoverNotification("rogulenkoko@gmail.com", "Кирилл Рогуленко", "123321");
                var success = service.TrySendNotificationAsync(message).Result;
                Assert.IsTrue(success);
            }
        }
    }

    public class FakeSettingsService : ISettingsService
    {
        public ISettings GetSettings()
        {
            return new FakeSettings();
        }
    }

    public class FakeSettings : ISettings
    {
        public bool EnableLogs => false;
        public string Email => ConfigurationManager.AppSettings["email"];
        public string EmailPassword => ConfigurationManager.AppSettings["emailPassword"];
        public LoggingType LoggingType => LoggingType.Empty;
    }
}
