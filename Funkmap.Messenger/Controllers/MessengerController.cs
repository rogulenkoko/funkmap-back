using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models.Requests;
using Funkmap.Messenger.Services;

namespace Funkmap.Messenger.Controllers
{
    [RoutePrefix("api/messenger")]
    [ValidateRequestModel]
    public class MessengerController : ApiController
    {
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessengerCacheService _messengerCache;

        public MessengerController(IDialogRepository dialogRepository, 
                                   IMessageRepository messageRepository,
                                   IMessengerCacheService messengerCache)
        {
            _dialogRepository = dialogRepository;
            _messageRepository = messageRepository;
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
            var dialogsEntities = await _dialogRepository.GetUserDialogs(parameter);
            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin)).ToList();
            return Content(HttpStatusCode.OK, dialogs);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogMessages")]
        public async Task<IHttpActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = Request.GetLogin();

            var parameter = new DialogMessagesParameter()
            {
                Skip = request.Skip,
                Take = request.Take,
                Members = new [] { request.Reciever, userLogin }
            };

            var messagesEntities = await _messageRepository.GetDilaogMessages(parameter);
            var messages = messagesEntities.Select(x => x.ToModel()).ToList();
            return Content(HttpStatusCode.OK, messages);
        }

        [HttpGet]
        [Authorize]
        [Route("getOnlineUsers")]
        public async Task<IHttpActionResult> GetDialogMessages()
        {
            var logins = _messengerCache.GetOnlineUsersLogins();
            return Content(HttpStatusCode.OK, logins);
        }
    }
}
