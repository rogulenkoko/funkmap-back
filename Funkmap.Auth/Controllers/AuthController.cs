using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Module.Auth.Abstract;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Controllers
{
    [RoutePrefix("api/auth")]
    [ValidateRequestModel]
    public class AuthController : ApiController
    {
        private readonly IRegistrationContextManager _contextManager;
        private readonly IRestoreContextManager _restoreContextManager;

        public AuthController(IRegistrationContextManager contextManager, 
                              IRestoreContextManager restoreContextManager)
        {
            _contextManager = contextManager;
            _restoreContextManager = restoreContextManager;
        }

        [HttpGet]
        [Route("validate/{login}")]
        public async Task<IHttpActionResult> Register(string login)
        {
            var creationContetResult = await _contextManager.ValidateLogin(login);

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

            var codeSentresult = await _contextManager.TryCreateContextAsync(request);

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
            if (String.IsNullOrEmpty(request.Email) || !(new EmailAddressAttribute().IsValid(request.Email)))
            {
                return BadRequest("invalid email");
            }

            var confirmationResult = await _contextManager.TryConfirmAsync(request.Login, request.Email, request.Code);

            var response = new RegistrationResponse()
            {
                Success = confirmationResult
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("restore/{email}")]
        public async Task<IHttpActionResult> AskRestore(string email)
        {
            var result = await _restoreContextManager.TryCreateRestoreContextAsync(email);

            if (!result)
            {
                return Ok(new BaseResponse() {Success = false});
            }

            return Ok(new BaseResponse() {Success = true});
        }

        [HttpPost]
        [Route("confirmRestore")]
        public async Task<IHttpActionResult> ConfirmRestore(ConfirmRestoreRequest request)
        {
            var result = await _restoreContextManager.TryConfirmRestoreAsync(request.LoginOrEmail, request.Code, request.Password);

            if (!result)
            {
                return Ok(new BaseResponse() { Success = false });
            }

            return Ok(new BaseResponse() { Success = true });
        }
    }
}
