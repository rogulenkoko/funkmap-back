using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Common.Auth;
using Funkmap.Common.Filters;
using Funkmap.Messenger.Data.Entities;
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
        private readonly UserService _userService;

        public MessengerController(IDialogRepository dialogRepository, 
                                   IMessengerCacheService messengerCache,
                                   UserService userService)
        {
            _dialogRepository = dialogRepository;
            _messengerCache = messengerCache;
            _userService = userService;
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

        [HttpGet]
        [Authorize]
        [Route("getNewDialogMessages")]
        public async Task<IHttpActionResult> GetNewDialogMessages()
        {

            var login = Request.GetLogin();
            var request = new UserLastVisitDateRequest()
            {
                Login = login
            };
            var date = _userService.GetLastVisitDate(request).LastVisitDateUtc;

            if (!date.HasValue)
            {
                return Ok(new List<DialogEntity>());
            }

            var parameter = new DialogsWithNewMessagesParameter()
            {
                Login = login,
                LastVisitDate = date.Value
            };
            ICollection<DialogEntity> dialogsWithNewMessages = await _dialogRepository.GetDialogsWithNewMessagesAsync(parameter);
            return Ok(dialogsWithNewMessages);

        }
    }
}
