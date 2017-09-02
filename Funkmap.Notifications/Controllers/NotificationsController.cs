using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;

namespace Funkmap.Notifications.Controllers
{
    [RoutePrefix("api/notifications")]
    [ValidateRequestModel]
    public class NotificationsController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("getNotifications")]
        public IHttpActionResult GetDialogMessages()
        {
            var login = Request.GetLogin();
            return Content(HttpStatusCode.OK, 1);
        }
    }
}
