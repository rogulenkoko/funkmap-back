using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Auth;
using Funkmap.Common.Owin.Filters;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Models;
using Funkmap.Notifications.Domain.Services.Abstract;
using NotificationAnswer = Funkmap.Notifications.Contracts.NotificationAnswer;

namespace Funkmap.Notifications.Controllers
{
    [RoutePrefix("api/notifications")]
    [ValidateRequestModel]
    public class NotificationsController : ApiController
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationRepository notificationRepository,
                                        INotificationService notificationService)
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
            
            var back = new NotificationAnswer()
            {
                Notification = notification.InnerNotification,
                Answer = answer.Answer
            };
            _notificationService.PublishNotificationAnswer(back);

            var response = new BaseResponse() {Success = true};
            return Content(HttpStatusCode.OK, response);
        }
    }
}
