using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Filters;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Module.Auth.Models;
using Funkmap.Module.Auth.Notifications;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    [ValidateRequestModel]
    public class AuthController : ApiController
    {
        private readonly IAuthRepository _authRepository;
        private readonly INotificationService _notificationService;

        private static readonly ConcurrentDictionary<string, UserConfirmationModel> _usersConfirmationCache = new ConcurrentDictionary<string, UserConfirmationModel>();
        public AuthController(IAuthRepository authRepository, INotificationService notificationService)
        {
            _authRepository = authRepository;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegistrationRequest creds)
        {
            var isExist = await _authRepository.CheckIfExist(creds.Login);
            isExist = isExist || _usersConfirmationCache.ContainsKey(creds.Login);

            var response = new RegistrationResponse()
            {
                Success = !isExist
            };

            if (!isExist)
            {
                _usersConfirmationCache[creds.Login] = new UserConfirmationModel()
                {
                    User = new UserEntity() { Login = creds.Login, Password = creds.Password,  Name = creds.Name}
                };
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("sendEmail")]
        public async Task<IHttpActionResult> SendEmail(RegistrationRequest request)
        {
            var response = new RegistrationResponse();
            if (!_usersConfirmationCache.ContainsKey(request.Login)) return Ok(response);

            _usersConfirmationCache[request.Login].User.Email = request.Email;

            var code = new Random().Next(100000, 999999).ToString();

            var notification = new ConfirmationNotification(request.Email, request.Name, code);
            var sendResult = await _notificationService.SendNotification(notification);
            _usersConfirmationCache[request.Login].Code = code;
            response.Success = sendResult;

            return Ok(response);
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<IHttpActionResult> Confirm(ConfirmationRequest request)
        {
            var response = new RegistrationResponse();
            if (!(_usersConfirmationCache.ContainsKey(request.Login) && _usersConfirmationCache[request.Login].Code == request.Code)) return Ok(response);

            var userConfirm = _usersConfirmationCache[request.Login];
            await _authRepository.CreateAsync(userConfirm.User);
            response.Success = true;


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
            var sendResult = await _notificationService.SendNotification(notification);
            response.Success = sendResult;
            return Ok(response);
        }
    }
}
