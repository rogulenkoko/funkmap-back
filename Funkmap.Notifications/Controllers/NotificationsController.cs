using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Mappers;

namespace Funkmap.Notifications.Controllers
{
    [RoutePrefix("api/notifications")]
    [ValidateRequestModel]
    public class NotificationsController : ApiController
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("getNotifications")]
        public async Task<IHttpActionResult> GetDialogMessages()
        {
            var login = Request.GetLogin();

            var notifications = await _notificationRepository.GetUserNotificationsAsync(login);
            var result = notifications.Select(x => x.ToNotification()).ToList();
            return Content(HttpStatusCode.OK, result);
        }
    }
}
