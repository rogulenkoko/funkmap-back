using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Auth
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void SendEmailTest()
        {
            var service = new EmailNotificationService();
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

    public class ConfirmationNotification : Notification
    {
        
    }
}
