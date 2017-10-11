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
using Funkmap.Tools;

namespace Funkmap.Messenger.Controllers
{
    [RoutePrefix("api/messenger")]
    [ValidateRequestModel]
    public class MessengerController : ApiController
    {
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessageRepository _messageRepository;

        private readonly IMessengerConnectionService _messengerConnection;

        public MessengerController(IDialogRepository dialogRepository,
                                   IMessageRepository messageRepository,
                                   IMessengerConnectionService messengerConnection)
        {
            _dialogRepository = dialogRepository;
            _messengerConnection = messengerConnection;
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

            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin, lastDialogMessage.FirstOrDefault(y => y.DialogId.ToString() == x.Id.ToString()).ToModel())).ToList();
            return Content(HttpStatusCode.OK, dialogs);
        }

        [HttpPost]
        [Authorize]
        [Route("createDialog")]
        public async Task<IHttpActionResult> CreateDialog(Dialog dialog)
        {
            if (dialog.Participants == null || dialog.Participants.Count == 0) return BadRequest("Invalid parameter");

            var login = Request.GetLogin();
            if (!dialog.Participants.Contains(login))
            {
                dialog.Participants.Add(login);
            }

            var now = DateTime.UtcNow;



            DialogEntity dialogEntity = dialog.ToEntity();
            dialogEntity.LastMessageDate = now;


            if (dialogEntity.Participants.Count == 2)
            {
                var isExist = await _dialogRepository.CheckDialogExist(dialogEntity.Participants);
                if (isExist) return Ok(new DialogResponse() { Success = false });
            }

            var id = await _dialogRepository.CreateAndGetIdAsync(dialogEntity);

            dialogEntity.Id = id;

            var response = new DialogResponse()
            {
                Success = true,
                Dialog = dialogEntity.ToModel(login, null)
            };

            if (dialog.Participants.Count > 2)
            {
                var message = new MessageEntity()
                {
                    DateTimeUtc = now,
                    DialogId = id,
                    Sender = "",
                    IsRead = false,
                    ToParticipants = dialog.Participants,
                    Text =
                        $"{login} создал беседу {dialog.Name} из {dialog.Participants.Count} участников" //todo подумать о локализации
                };
                await _messageRepository.AddMessage(message);
                response.Dialog.LastMessage = message.ToModel();
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [Route("updateDialog")]
        public async Task<IHttpActionResult> UpdateDialog(Dialog dialog)
        {
            var login = Request.GetLogin();
            var exsitingDialog = await _dialogRepository.GetAsync(dialog.DialogId);

            if (exsitingDialog == null) return BadRequest("Dialog not exists");

            var response = new DialogResponse()
            {
                Success = true,
                Dialog = exsitingDialog.ToModel(login)
            };

            var now = DateTime.UtcNow;
            var newDialogEntity = dialog.ToEntity();

            var addedParticipants = newDialogEntity.Participants.Except(exsitingDialog.Participants).ToList();
            if (addedParticipants.Any())
            {
                exsitingDialog.LastMessageDate = now;
                string addedParticipantsString = addedParticipants.Count == 1 ? addedParticipants.First() : String.Join(", ", addedParticipants);
                var message = new MessageEntity()
                {
                    DateTimeUtc = now,
                    DialogId = exsitingDialog.Id,
                    Sender = "",
                    IsRead = false,
                    ToParticipants = exsitingDialog.Participants.Except(new List<string>() { login }).ToList(),
                    Text = $"{login} пригласил {addedParticipantsString}"//todo подумать о локализации
                };
                await _messageRepository.AddMessage(message);
                response.Dialog.LastMessage = message.ToModel();
            }

            exsitingDialog = exsitingDialog.FillEntity(newDialogEntity);

            await _dialogRepository.UpdateAsync(exsitingDialog);



            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogMessages")]
        public async Task<IHttpActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = Request.GetLogin();
            if (String.IsNullOrEmpty(request.DialogId)) return BadRequest("dialogId is empty");

            //проверить является ли этот пользователем участником диалога
            var parameter = new DialogMessagesParameter()
            {
                Skip = request.Skip,
                Take = request.Take,
                DialogId = request.DialogId,
                UserLogin = userLogin
            };

            var dialogExist = await _dialogRepository.CheckDialogExist(parameter.DialogId);
            if (!dialogExist) return Content(HttpStatusCode.OK, new List<Message>());

            var messagesEntities = await _messageRepository.GetDialogMessagesAsync(parameter);
            var messages = messagesEntities.Select(x => x.ToModel()).ToList();
            return Content(HttpStatusCode.OK, messages);
        }

        [HttpGet]
        [Authorize]
        [Route("getOnlineUsers")]
        public IHttpActionResult GetDialogMessages()
        {
            var logins = _messengerConnection.GetOnlineUsersLogins();
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
                DialogIds = dialogs.Select(x => x.Id.ToString()).ToList()
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

            dialogIds = dialogIds.Where(x => !String.IsNullOrEmpty(x)).ToArray();
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
