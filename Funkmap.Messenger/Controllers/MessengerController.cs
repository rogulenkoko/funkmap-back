using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Models.Requests;
using Funkmap.Messenger.Services;

namespace Funkmap.Messenger.Controllers
{
    [RoutePrefix("api/messenger")]
    [ValidateRequestModel]
    public class MessengerController : ApiController
    {
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessengerCacheService _messengerCache;

        public MessengerController(IDialogRepository dialogRepository, 
                                   IMessengerCacheService messengerCache)
        {
            _dialogRepository = dialogRepository;
            _messengerCache = messengerCache;
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogs")]
        public async Task<IHttpActionResult> GetDialogs(DialogsRequest request)
        {
            var userLogin = Request.GetLogin();
            var parameter = new UserDialogsParameter()
            {
                Login = userLogin,
                Skip = request.Skip,
                Take = request.Take
            };
            var dialogsEntities = await _dialogRepository.GetUserDialogsAsync(parameter);
            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin)).ToList();
            return Content(HttpStatusCode.OK, dialogs);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogMessages")]
        public async Task<IHttpActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = Request.GetLogin();


            //проверить является ли этот пользователем участником диалога
            var parameter = new DialogMessagesParameter()
            {
                Skip = request.Skip,
                Take = request.Take,
                DialogId = request.DialogId
            };

            var messagesEntities = await _dialogRepository.GetDialogMessagesAsync(parameter);
            var messages = messagesEntities.Select(x => x.ToModel()).ToList();
            return Content(HttpStatusCode.OK, messages);
        }

        [HttpGet]
        [Authorize]
        [Route("getOnlineUsers")]
        public IHttpActionResult GetDialogMessages()
        {
            var logins = _messengerCache.GetOnlineUsersLogins();
            return Content(HttpStatusCode.OK, logins);
        }
    }
}
