using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Models.Requests;
using Funkmap.Messenger.Models.Responses;
using Funkmap.Messenger.Services.Abstract;
using Funkmap.Tools;

namespace Funkmap.Messenger.Controllers
{
    [RoutePrefix("api/messenger")]
    [ValidateRequestModel]
    public class MessengerController : ApiController
    {
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ICommandBus _commandBus;

        private readonly IMessengerConnectionService _messengerConnection;

        public MessengerController(IDialogRepository dialogRepository,
                                   IMessageRepository messageRepository,
                                   IMessengerConnectionService messengerConnection,
                                   ICommandBus commandBus)
        {
            _dialogRepository = dialogRepository;
            _messengerConnection = messengerConnection;
            _messageRepository = messageRepository;
            _commandBus = commandBus;
        }

        [HttpGet]
        [Authorize]
        [Route("getDialogs")]
        public async Task<IHttpActionResult> GetDialogs()
        {
            var userLogin = Request.GetLogin();
            var dialogsEntities = await _dialogRepository.GetUserDialogsAsync(userLogin);
            if (dialogsEntities == null || dialogsEntities.Count == 0) return Ok(new List<Dialog>());

            var dialogs = dialogsEntities.Select(x => x.ToModel(userLogin)).ToList();
            return Content(HttpStatusCode.OK, dialogs);
        }

        [HttpGet]
        [Authorize]
        [Route("getDialogAvatar/{dialogId}")]
        public async Task<IHttpActionResult> GetDialogAvatar(string dialogId)
        {
            var dialog = await _dialogRepository.GetDialogAvatarAsync(dialogId);
            var avatarInfo = dialog.ToAvatarInfo();
            return Ok(avatarInfo);
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogsAvatars")]
        public async Task<IHttpActionResult> GetDialogsAvatars(string[] dialogIds)
        {
            var distinctIds = dialogIds.Distinct().ToArray();
            var dialogs = await _dialogRepository.GetDialogsAvatarsAsync(distinctIds);
            var avatarsInfo = dialogs.Select(x => x.ToAvatarInfo()).ToArray();
            return Ok(avatarsInfo);
        }

        [HttpPost]
        [Authorize]
        [Route("createDialog")]
        public IHttpActionResult CreateDialog(Dialog dialog)
        {
            var login = Request.GetLogin();
            var command = new CreateDialogCommand()
            {
                Participants = dialog.Participants,
                CreatorLogin = login,
                DialogName = dialog.Name
            };

            _commandBus.Execute(command);

            return Ok(new DialogResponse() {Success = true});
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

            if (newDialogEntity.Avatar != null)
            {
                var cutted = FunkmapImageProcessor.MinifyImage(newDialogEntity.Avatar.Image.AsByteArray);
                newDialogEntity.Avatar.Image = cutted;
            }

            if (newDialogEntity.Participants != null && newDialogEntity.Participants.Except(exsitingDialog.Participants).Any())
            {
                var addedParticipants = newDialogEntity.Participants.Except(exsitingDialog.Participants).ToList();
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
        [Route("leaveDialog")]
        public async Task<IHttpActionResult> LeaveDialog(LeaveDialogRequest request)
        {
            var userLogin = Request.GetLogin();

            var dialog = await _dialogRepository.GetAsync(request.DialogId);

            if (dialog.CreatorLogin != userLogin && userLogin != request.Login) return BadRequest("you can't do it");

            if ((dialog.CreatorLogin == userLogin) || (userLogin == request.Login))
            {
                if (dialog.Participants.Any() && dialog.Participants.Contains(request.Login))
                {
                    dialog.Participants.Remove(request.Login);
                    await _dialogRepository.UpdateAsync(dialog);

                    var now = DateTime.UtcNow;

                    MessageEntity message;

                    if (dialog.CreatorLogin == userLogin && userLogin != request.Login)
                    {
                        message = new MessageEntity()
                        {
                            DateTimeUtc = now,
                            DialogId = dialog.Id,
                            ToParticipants = dialog.Participants,
                            Sender = "",
                            Text = $"{userLogin} исключил {request.Login} из беседы"
                        };
                    }
                    else
                    {
                        message = new MessageEntity()
                        {
                            DateTimeUtc = now,
                            DialogId = dialog.Id,
                            ToParticipants = dialog.Participants,
                            Sender = "",
                            Text = $"{userLogin} покинул беседу"
                        };

                        dialog.LastMessage = message;
                    }

                    await _messageRepository.AddMessage(message);



                    return Ok(new DialogResponse() { Success = true, Dialog = dialog.ToModel(userLogin)});
                }
            }


            return Ok(new DialogResponse() { Success = false });

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
