using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Funkmap.Common.Core.Auth;
using Funkmap.Common.Models;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification = Funkmap.Notifications.Domain.Models.Notification;
using NotificationAnswer = Funkmap.Notifications.Contracts.Models.NotificationAnswer;

namespace Funkmap.Notifications.Controllers
{
    [Route("api/notifications")]
    public class NotificationsController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IFunkmapNotificationService _notificationService;

        public NotificationsController(INotificationRepository notificationRepository,
                                       IFunkmapNotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get all existing users's notifications.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<Notification>))]
        [Route("")]
        public async Task<IActionResult> GetNotifications()
        {
            var login = User.GetLogin();

            var notifications = await _notificationRepository.GetUserNotificationsAsync(login);
            return Ok(notifications);
        }

        /// <summary>
        /// Get count of fresh notifications.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(int))]
        [Route("new/count")]
        public async Task<ActionResult> GetNewNotificationsCount()
        {
            var login = User.GetLogin();

            var notificationsCount = await _notificationRepository.GetNewNotificationsCountAsync(login);
            return Ok(notificationsCount);
        }

        /// <summary>
        /// Answer notification if it's possible.
        /// </summary>
        /// <param name="answer"></param>
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(BaseResponse))]
        [Route("answer")]
        public async Task<IActionResult> AnswerNotification(NotificationAnswerRequest answer)
        {
            var notification = await _notificationRepository.GetAsync(answer.NotificationId);
            
            var back = new NotificationAnswer
            {
                NotificationJson = notification.InnerNotificationJson,
                Answer = answer.Answer,
                Sender = notification.ReceiverLogin,
                Receiver = notification.SenderLogin,
                NotificationType = notification.NotificationType
            };
            await _notificationService.AnswerAsync(back);

            var response = new BaseResponse() {Success = true};
            return Ok(response);
        }
    }
}
