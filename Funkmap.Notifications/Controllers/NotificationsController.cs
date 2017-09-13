using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Mappers;
using Funkmap.Notifications.Models;
using Funkmap.Notifications.Services.Abstract;

namespace Funkmap.Notifications.Controllers
{
    [RoutePrefix("api/notifications")]
    [ValidateRequestModel]
    public class NotificationsController : ApiController
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnumerable<INotificationsService> _notificationsServices;

        public NotificationsController(INotificationRepository notificationRepository,
                                        IEnumerable<INotificationsService> notificationsServices)
        {
            _notificationRepository = notificationRepository;
            _notificationsServices = notificationsServices;
        }

        [HttpGet]
        [Authorize]
        [Route("getNotifications")]
        public async Task<IHttpActionResult> GetNotifications()
        {
            var login = Request.GetLogin();

            var notifications = await _notificationRepository.GetUserNotificationsAsync(login);
            var result = notifications.Select(x => x.ToNotificationModel()).ToList();
            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Authorize]
        [Route("getNewNotificationsCount")]
        public async Task<IHttpActionResult> GetNewNotificationsCount()
        {
            var login = Request.GetLogin();

            var notificationsCount = await _notificationRepository.GetNewNotificationsCount(login);
            return Content(HttpStatusCode.OK, notificationsCount);
        }


        [HttpPost]
        [Authorize]
        [Route("answer")]
        public async Task<IHttpActionResult> AnswerNotification(NotificationAnswer answer)
        {

            var notification = await _notificationRepository.GetAsync(answer.NotificationId);
            await _notificationRepository.DeleteAsync(answer.NotificationId);

            var notificationService = _notificationsServices.SingleOrDefault(x=>x.NotificationType == notification.NotificationType);
            if (notificationService == null) return BadRequest("notification handler not found");

            var back = new NotificationBack()
            {
                Notification = notification.ToNotification(),
                Answer = answer.Answer
            };
            notificationService.PublishBackRequest(back);

            var response = new BaseResponse() {Success = true};
            return Content(HttpStatusCode.OK, response);
        }
    }
}
