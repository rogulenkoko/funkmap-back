using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Funkmap.Common.Auth;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Models.Requests;
using Funkmap.Messenger.Models.Responses;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Services.Abstract;

namespace Funkmap.Messenger.Controllers
{
    [RoutePrefix("api/messenger")]
    [ValidateRequestModel]
    public class MessengerController : ApiController
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
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(BaseResponse))]
        [Route("message")]
        public IHttpActionResult SendMessage(MessageModel message)
        {
            var response = new BaseResponse();

            var login = Request.GetLogin();

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
            _commandBus.ExecuteAsync<SaveMessageCommand>(command);

            response.Success = true;
            return Ok(response);
        }

        /// <summary>
        /// Start uploading dialog content (you can get uploading status from SignalR hub as a push-notification)
        /// </summary>
        /// <param name="contentItemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(BaseResponse))]
        [Route("content")]
        public IHttpActionResult StartUploadContent(ContentItemModel contentItemModel)
        {
            var login = Request.GetLogin();
            var command = new StartUploadContentCommand(contentItemModel.ContentType, contentItemModel.Name, contentItemModel.Data, login);

            _commandBus.ExecuteAsync(command);

            return Ok(new BaseResponse() {Success = true});
        }

        /// <summary>
        /// Get all users's (not empty) dialogs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("dialogs")]
        public async Task<IHttpActionResult> GetDialogs()
        {
            var userLogin = Request.GetLogin();

            var query = new UserDialogsQuery(userLogin);

            var response = await _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<DialogModel>());
            }

            return Content(HttpStatusCode.OK, response.Dialogs.Select(x => x.ToModel(userLogin)));
        }

        /// <summary>
        /// Get some dialog messages.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("dialog/messages")]
        public async Task<IHttpActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = Request.GetLogin();
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
            return Content(HttpStatusCode.OK, messages);
        }


        /// <summary>
        /// Get dialog's avatar (bytes or base64).
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(DialogAvatarInfoModel))]
        [Route("dialog/{dialogId}/avatar")]
        public async Task<IHttpActionResult> GetDialogAvatar(string dialogId)
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
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("dialogs/online")]
        public IHttpActionResult GetDialogMessages()
        {
            var logins = _messengerConnection.GetOnlineUsersLogins();
            return Content(HttpStatusCode.OK, logins);
        }

        /// <summary>
        /// Get dialogs with fresh messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<DialogsNewMessagesCountModel>))]
        [Route("dialogs/new")]
        public async Task<IHttpActionResult> GetDialogsNewMessagesCount()
        {
            var login = Request.GetLogin();

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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("dialog")]
        public IHttpActionResult CreateDialog(CreateDialogRequest request)
        {
            var login = Request.GetLogin();
            var command = new CreateDialogCommand()
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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("dialog/participants")]
        public IHttpActionResult InviteParticipants(InviteParticipantsRequest request)
        {
            var login = Request.GetLogin();
            _commandBus.ExecuteAsync(new InviteParticipantsCommand(login, request.InvitedUserLogins, request.DialogId));
            return Ok(new BaseResponse() { Success = true });
        }

        /// <summary>
        /// Update dialog (avatar or name).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("dialog")]
        public IHttpActionResult UpdateDialogInfo(DialogUpdateRequest request)
        {
            var login = Request.GetLogin();

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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("dialog/leave")]
        public IHttpActionResult LeaveDialog(LeaveDialogRequest request)
        {
            var userLogin = Request.GetLogin();

            _commandBus.ExecuteAsync(new LeaveDialogCommand(request.DialogId, request.Login, userLogin));

            return Ok(new DialogResponse() { Success = true });
        }

    }
}
