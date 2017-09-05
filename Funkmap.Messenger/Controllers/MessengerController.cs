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
using Funkmap.Messenger.Models.Responses;
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

        [HttpGet]
        [Authorize]
        [Route("getDialogs")]
        public async Task<IHttpActionResult> GetDialogs()
        {
            var userLogin = Request.GetLogin();
            var dialogsEntities = await _dialogRepository.GetUserDialogsAsync(userLogin);
            if (dialogsEntities == null || dialogsEntities.Count == 0) return Ok(new List<Dialog>());

            var dialogIds = dialogsEntities.Select(x => x.Id.ToString()).ToArray();
            var lastDialogMessage = await _messageRepository.GetLastDialogsMessages(dialogIds);

            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin, lastDialogMessage.FirstOrDefault(y=>y.DialogId.ToString() == x.Id.ToString()).ToModel())).ToList();
            return Content(HttpStatusCode.OK, dialogs);
        }

        [HttpPost]
        [Authorize]
        [Route("createDialog")]
        public async Task<IHttpActionResult> CreateDialog(Dialog dialog)
        {
            if (dialog.Participants == null || dialog.Participants.Count < 2) return BadRequest("Invalid parameter");

            var isExist = await _dialogRepository.IsDialogExist(dialog.Participants);

            if (isExist) return BadRequest("Dialog exists");

            var id = await _dialogRepository.CreateAsync(dialog.ToEntity());
            var response = new CreateDialogResponse()
            {
                Success = true,
                DialogId = id.ToString()
            };

            return Ok(response);
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
                DialogId = request.DialogId,
                UserLogin = userLogin
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
            var dialogs = await _dialogRepository.GetUserDialogsAsync(login);
            if (dialogs == null || dialogs.Count == 0) return Ok(0);

            var messagesParameter = new DialogsNewMessagesParameter()
            {
                Login = login,
                DialogIds = dialogs.Select(x=>x.Id.ToString()).ToList()
            };
            var dialogsWithNewMessages = await _messageRepository.GetDialogsWithNewMessagesAsync(messagesParameter);

            return Ok(dialogsWithNewMessages);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogsNewMessagesCount")]
        public async Task<IHttpActionResult> GetDialogsNewMessagesCount(string[] dialogIds)
        {
            var login = Request.GetLogin();
            if (dialogIds.Length == 0) return Ok(new List<DialogsNewMessagesCountModel>());
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
