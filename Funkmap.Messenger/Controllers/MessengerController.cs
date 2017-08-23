using System;
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
        private readonly IMessageRepository _messageRepository;

        private readonly IMessengerCacheService _messengerCache;
        private readonly UserService _userService;

        public MessengerController(IDialogRepository dialogRepository,
                                   IMessageRepository messageRepository,
                                   IMessengerCacheService messengerCache,
                                   UserService userService)
        {
            _dialogRepository = dialogRepository;
            _messengerCache = messengerCache;
            _userService = userService;
            _messageRepository = messageRepository;
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
           // var lastDialogMessage = await _messageRepository.ge

            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin, null)).ToList();
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

            var messagesEntities = await _messageRepository.GetDialogMessagesAsync(parameter);
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
        [Route("getDialogsWithNewMessagesCount")]
        public async Task<IHttpActionResult> GetNewDialogMessages()
        {

            var login = Request.GetLogin();
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = login,
                Skip = 0,
                Take = Int32.MaxValue
            };
            var dialogs = await _dialogRepository.GetUserDialogsAsync(dialogsParameter);
            if (dialogs == null || dialogs.Count == 0) return Ok(0);

            var messagesParameter = new DialogsNewMessagesParameter()
            {
                Login = login,
                DialogIds = dialogs.Select(x=>x.Id.ToString()).ToList()
            };
            var count = await _messageRepository.GetDialogsWithNewMessagesCountAsync(messagesParameter);

            return Ok(count);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogNewMessagesCount")]
        public async Task<IHttpActionResult> GetDialogNewMessagesCount(string[] dialogIds)
        {
            var login = Request.GetLogin();
            var parameter = new DialogsNewMessagesParameter()
            {
                Login = login,
                DialogIds = dialogIds
            };
            var messagesCount = await _messageRepository.GetDialogNewMessagesCount(parameter);
            var result = messagesCount.Select(x => x.ToModel());
            return Ok(result);
        }
    }
}
