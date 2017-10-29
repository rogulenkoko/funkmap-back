using System.Linq;
using System.Net;
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
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationRepository notificationRepository,
                                        INotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
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

            var notificationsCount = await _notificationRepository.GetNewNotificationsCountAsync(login);
            return Content(HttpStatusCode.OK, notificationsCount);
        }


        [HttpPost]
        [Authorize]
        [Route("answer")]
        public async Task<IHttpActionResult> AnswerNotification(NotificationAnswerModel answer)
        {

            var notification = await _notificationRepository.GetAsync(answer.NotificationId);
            await _notificationRepository.DeleteAsync(answer.NotificationId);
            
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
