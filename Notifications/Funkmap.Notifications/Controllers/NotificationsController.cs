using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Filters;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Models;
using Notification = Funkmap.Notifications.Domain.Models.Notification;
using NotificationAnswer = Funkmap.Notifications.Contracts.Models.NotificationAnswer;

namespace Funkmap.Notifications.Controllers
{
    [RoutePrefix("api/notifications")]
    [ValidateRequestModel]
    public class NotificationsController : ApiController
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
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<Notification>))]
        [Route("")]
        public async Task<IHttpActionResult> GetNotifications()
        {
            var login = Request.GetLogin();

            var notifications = await _notificationRepository.GetUserNotificationsAsync(login);
            return Content(HttpStatusCode.OK, notifications);
        }

        /// <summary>
        /// Get count of fresh notifications.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(int))]
        [Route("new/count")]
        public async Task<IHttpActionResult> GetNewNotificationsCount()
        {
            var login = Request.GetLogin();

            var notificationsCount = await _notificationRepository.GetNewNotificationsCountAsync(login);
            return Content(HttpStatusCode.OK, notificationsCount);
        }

        /// <summary>
        /// Answer notification if it's possible.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(BaseResponse))]
        [Route("answer")]
        public async Task<IHttpActionResult> AnswerNotification(NotificationAnswerRequest answer)
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
            return Content(HttpStatusCode.OK, response);
        }
    }
}
