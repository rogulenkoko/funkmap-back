using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Core.Auth;
using Funkmap.Common.Models;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Models.Requests;
using Funkmap.Messenger.Models.Responses;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Messenger.Controllers
{
    [Route("api/messenger")]
    public class MessengerController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryContext _queryContext;

        private readonly IMessengerConnectionService _messengerConnection;

        public MessengerController(IMessengerConnectionService messengerConnection,
                                   ICommandBus commandBus,
                                   IQueryContext queryContext)
        {
            _messengerConnection = messengerConnection;
            _commandBus = commandBus;
            _queryContext = queryContext;
        }

        /// <summary>
        /// Send message (you can get saved message from SignalR hub as a push-notification).
        /// </summary>
        /// <param name="message"><see cref="MessageModel"/></param>
        [HttpPost]
        [Authorize]
        [Route("message")]
        public IActionResult SendMessage(MessageModel message)
        {
            var response = new BaseResponse();

            var login = User.GetLogin();

            if (message.Sender != login) return BadRequest();

            var usersWithOpenedCurrentDialog = _messengerConnection.GetDialogParticipants(message.DialogId);
            var command = new SaveMessageCommand
            {
                DialogId = message.DialogId,
                Sender = message.Sender,
                Text = message.Text,
                Content = message.Content?.Select(x => x.ToEntity()).ToList(),
                UsersWithOpenedCurrentDialog = usersWithOpenedCurrentDialog
            };
            _commandBus.ExecuteAsync(command);

            response.Success = true;
            return Ok(response);
        }

        /// <summary>
        /// Start uploading dialog content (you can get uploading status from SignalR hub as a push-notification)
        /// </summary>
        /// <param name="contentItemModel"><see cref="ContentItemModel"/></param>
        [HttpPost]
        [Authorize]
        [Route("content")]
        public IActionResult StartUploadContent(ContentItemModel contentItemModel)
        {
            var login = User.GetLogin();
            var command = new StartUploadContentCommand(contentItemModel.ContentType, contentItemModel.Name, contentItemModel.Data, login);

            _commandBus.ExecuteAsync(command);

            return Ok(new BaseResponse() {Success = true});
        }

        /// <summary>
        /// Get all users's (not empty) dialogs.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("dialogs")]
        public async Task<IActionResult> GetDialogs()
        {
            var userLogin = User.GetLogin();

            var query = new UserDialogsQuery(userLogin);

            var response = await _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<DialogModel>());
            }

            return Ok(response.Dialogs.Select(x => x.ToModel(userLogin)));
        }

        /// <summary>
        /// Get some dialog messages.
        /// </summary>
        /// <param name="request"><see cref="DialogMessagesRequest"/></param>
        [HttpPost]
        [Authorize]
        [Route("dialog/messages")]
        public async Task<IActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = User.GetLogin();
            if (String.IsNullOrEmpty(request.DialogId)) return BadRequest("Dialog Id is empty.");

            var query = new DialogMessagesQuery()
            {
                Skip = request.Skip,
                Take = request.Take,
                DialogId = request.DialogId,
                UserLogin = userLogin
            };

            var response = await _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<Message>());
            }

            var messages = response.Messages.Select(x => x.ToModel());
            return Ok(messages);
        }


        /// <summary>
        /// Get dialog's avatar (bytes or base64).
        /// </summary>
        /// <param name="dialogId">Dialog id</param>
        [HttpGet]
        [Authorize]
        [Route("dialog/{dialogId}/avatar")]
        public async Task<IActionResult> GetDialogAvatar(string dialogId)
        {
            var query = new DialogAvatarInfoQuery()
            {
                DialogId = dialogId
            };

            var response = await _queryContext.ExecuteAsync<DialogAvatarInfoQuery, DialogAvatarInfoResponse>(query);

            if (!response.Success)
            {
                return null;
            }

            DialogAvatarInfoModel avatarInfo = new DialogAvatarInfoModel() { Id = response.DialogId, Bytes = response.AvatarBytes };
            return Ok(avatarInfo);
        }


        /// <summary>
        /// Get online users with who you have not empty dialog.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("dialogs/online")]
        public IActionResult GetDialogMessages()
        {
            var logins = _messengerConnection.GetOnlineUsersLogins();
            return Ok(logins);
        }

        /// <summary>
        /// Get dialogs with fresh messages.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("dialogs/new")]
        public async Task<IActionResult> GetDialogsNewMessagesCount()
        {
            var login = User.GetLogin();

            var query = new DialogsNewMessagesCountQuery(login);

            var response = await _queryContext.ExecuteAsync<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<DialogsNewMessagesCountModel>());
            }

            return Ok(response.CountResults.Select(x => x.ToModel()));
        }

        /// <summary>
        /// Create dialog (you can get created dialog Id from SignalR hub as a push-notification).
        /// </summary>
        /// <param name="request"><see cref="CreateDialogRequest"/></param>
        [HttpPost]
        [Authorize]
        [Route("dialog")]
        public IActionResult CreateDialog(CreateDialogRequest request)
        {
            var login = User.GetLogin();
            var command = new CreateDialogCommand
            {
                Participants = request.Participants,
                CreatorLogin = login,
                DialogName = request.Name
            };

            _commandBus.ExecuteAsync(command);

            return Ok(new DialogResponse() { Success = true });
        }

        /// <summary>
        /// Add participants to dialog.
        /// </summary>
        /// <param name="request"><see cref="InviteParticipantsRequest"/></param>
        [HttpPost]
        [Authorize]
        [Route("dialog/participants")]
        public IActionResult InviteParticipants(InviteParticipantsRequest request)
        {
            var login = User.GetLogin();
            _commandBus.ExecuteAsync(new InviteParticipantsCommand(login, request.InvitedUserLogins, request.DialogId));
            return Ok(new BaseResponse() { Success = true });
        }

        /// <summary>
        /// Update dialog (avatar or name).
        /// </summary>
        /// <param name="request"><see cref="DialogUpdateRequest"/></param>
        [HttpPut]
        [Authorize]
        [Route("dialog")]
        public IActionResult UpdateDialogInfo(DialogUpdateRequest request)
        {
            var login = User.GetLogin();

            _commandBus.ExecuteAsync(new UpdateDialogInfoCommand()
            {
                Avatar = request.Avatar,
                DialogId = request.DialogId,
                Name = request.Name,
                UserLogin = login
            });

            return Ok(new BaseResponse() { Success = true });
        }

        /// <summary>
        /// Leave dialog.
        /// </summary>
        /// <param name="request"><see cref="LeaveDialogRequest"/></param>
        [HttpPost]
        [Authorize]
        [Route("dialog/leave")]
        public IActionResult LeaveDialog(LeaveDialogRequest request)
        {
            var userLogin = User.GetLogin();

            _commandBus.ExecuteAsync(new LeaveDialogCommand(request.DialogId, request.Login, userLogin));

            return Ok(new DialogResponse { Success = true });
        }
    }
}
