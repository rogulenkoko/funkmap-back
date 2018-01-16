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

        [HttpGet]
        [Authorize]
        [Route("getDialogs")]
        public async Task<IHttpActionResult> GetDialogs()
        {
            var userLogin = Request.GetLogin();

            var query = new UserDialogsQuery(userLogin);

            var response = await _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<DialogModel>());
            }

            return Content(HttpStatusCode.OK, response.Dialogs.Select(x => x.ToModel(userLogin)));
        }

        [HttpPost]
        [Authorize]
        [Route("getDialogMessages")]
        public async Task<IHttpActionResult> GetDialogMessages(DialogMessagesRequest request)
        {
            var userLogin = Request.GetLogin();
            if (String.IsNullOrEmpty(request.DialogId)) return BadRequest("dialogId is empty");

            var query = new DialogMessagesQuery()
            {
                Skip = request.Skip,
                Take = request.Take,
                DialogId = request.DialogId,
                UserLogin = userLogin
            };

            var response = await _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<Message>());
            }

            var messages = response.Messages.Select(x => x.ToModel()).ToList();
            return Content(HttpStatusCode.OK, messages);
        }

        [HttpGet]
        [Authorize]
        [Route("getDialogAvatar/{dialogId}")]
        public async Task<IHttpActionResult> GetDialogAvatar(string dialogId)
        {
            var query = new DialogAvatarInfoQuery()
            {
                DialogId = dialogId
            };

            var response = await _queryContext.Execute<DialogAvatarInfoQuery, DialogAvatarInfoResponse>(query);

            if (!response.Success)
            {
                return null;
            }

            DialogAvatarInfoModel avatarInfo = new DialogAvatarInfoModel() { Id = response.DialogId, Bytes = response.AvatarBytes };
            return Ok(avatarInfo);
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
        public async Task<IHttpActionResult> GetDialogsNewMessagesCount()
        {
            var login = Request.GetLogin();

            var query = new DialogsNewMessagesCountQuery(login);

            var response = await _queryContext.Execute<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>(query);

            if (!response.Success)
            {
                return Ok(new List<DialogsNewMessagesCountModel>());
            }

            return Ok(response.CountResults.Select(x => x.ToModel()));
        }

        [HttpPost]
        [Authorize]
        [Route("createDialog")]
        public IHttpActionResult CreateDialog(CreateDialogRequest request)
        {
            var login = Request.GetLogin();
            var command = new CreateDialogCommand()
            {
                Participants = request.Participants,
                CreatorLogin = login,
                DialogName = request.Name
            };

            _commandBus.Execute(command);

            return Ok(new DialogResponse() { Success = true });
        }

        [HttpPost]
        [Authorize]
        [Route("inviteParticipants")]
        public IHttpActionResult InviteParticipants(InviteParticipantsRequest request)
        {
            var login = Request.GetLogin();
            _commandBus.Execute(new InviteParticipantsCommand(login, request.InvitedUserLogins, request.DialogId));
            return Ok(new BaseResponse() { Success = true });
        }

        [HttpPost]
        [Authorize]
        [Route("updateDialog")]
        public IHttpActionResult UpdateDialogInfo(DialogUpdateRequest request)
        {
            var login = Request.GetLogin();

            _commandBus.Execute(new UpdateDialogInfoCommand()
            {
                Avatar = request.Avatar,
                DialogId = request.DialogId,
                Name = request.Name,
                UserLogin = login
            });

            return Ok(new BaseResponse() { Success = true });
        }

        [HttpPost]
        [Authorize]
        [Route("leaveDialog")]
        public IHttpActionResult LeaveDialog(LeaveDialogRequest request)
        {
            var userLogin = Request.GetLogin();

            _commandBus.Execute(new LeaveDialogCommand(request.DialogId, request.Login, userLogin));

            return Ok(new DialogResponse() { Success = true });
        }

    }
}
