using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Module.Auth.Models;
using Funkmap.Module.Auth.Notifications;
using Funkmap.Module.Auth.Services;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    [ValidateRequestModel]
    public class AuthController : ApiController
    {
        private readonly IAuthRepository _authRepository;
        private readonly IExternalNotificationService _externalNotificationService;
        private readonly IRegistrationContextManager _contextManager;
        public AuthController(IAuthRepository authRepository, IExternalNotificationService externalNotificationService,
            IRegistrationContextManager contextManager)
        {
            _authRepository = authRepository;
            _externalNotificationService = externalNotificationService;
            _contextManager = contextManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegistrationRequest creds)
        {
            var creationContetResult = await _contextManager.TryCreateContextAsync(creds);

            var response = new RegistrationResponse()
            {
                Success = creationContetResult
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("sendEmail")]
        public async Task<IHttpActionResult> SendEmail(RegistrationRequest request)
        {
            if (String.IsNullOrEmpty(request.Email) || !(new EmailAddressAttribute().IsValid(request.Email)))
            {
                return BadRequest("invalid email");
            }

            var codeSentresult = await _contextManager.TrySendCodeAsync(request.Login, request.Email);

            var response = new BaseResponse
            {
                Success = codeSentresult
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<IHttpActionResult> Confirm(ConfirmationRequest request)
        {
            var confirmationResult = await _contextManager.TryConfirmAsync(request.Login, request.Code);

            var response = new RegistrationResponse()
            {
                Success = confirmationResult
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("restore")]
        public async Task<IHttpActionResult> SendEmailForКecovery(String email)
        {
            var response = new RegistrationResponse();
            UserEntity user = _authRepository.GetUserByEmail(email).Result;
            if (user == null) return Ok(response);
            var notification = new PasswordRecoverNotification(email, user.Name, user.Password);
            var sendResult = await _externalNotificationService.TrySendNotificationAsync(notification);
            response.Success = sendResult;
            return Ok(response);
        }
    }
}
