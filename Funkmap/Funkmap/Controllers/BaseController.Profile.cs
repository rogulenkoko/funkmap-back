using System.Net.Http;
using System.Threading.Tasks;
using Funkmap.Common.Core.Auth;
using Funkmap.Common.Core.Extensions;
using Funkmap.Common.Models;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Mappers;
using Funkmap.Models.Requests;
using Funkmap.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Controllers
{
    public partial class BaseController
    {
        /// <summary>
        /// Get full profile information.
        /// </summary>
        /// <param name="login">Profile login</param>
        [HttpGet]
        [Route("profile/{login}")]
        public async Task<IActionResult> GetFull(string login)
        {
            var entity = await _queryRepository.GetAsync(login);
            if (entity == null)
            {
                return BadRequest("Invalid profile login.");
            }

            Request.SetProfileCorrectAvatarUrls(entity);
            return Ok(entity);
        }

        /// <summary>
        /// Create a musician.
        /// </summary>
        /// <param name="model"><see cref="Musician"/></param>
        [Authorize]
        [HttpPost]
        [Route("musician")]
        public Task<IActionResult> Create(Musician model) => CreateAsync(model);
        
        /// <summary>
        /// Create a band.
        /// </summary>
        /// <param name="model"><see cref="Band"/></param>
        [Authorize]
        [HttpPost]
        [Route("band")]
        public Task<IActionResult> Create(Band model) => CreateAsync(model);
        
        /// <summary>
        /// Create a shop.
        /// </summary>
        /// <param name="model"><see cref="Shop"/></param>
        [Authorize]
        [HttpPost]
        [Route("shop")]
        public Task<IActionResult> Create(Shop model) => CreateAsync(model);
        
        /// <summary>
        /// Create a studio.
        /// </summary>
        /// <param name="model"><see cref="Studio"/></param>
        [Authorize]
        [HttpPost]
        [Route("studio")]
        public Task<IActionResult> Create(Studio model) => CreateAsync(model);
        
        /// <summary>
        /// Create a rehearsal point.
        /// </summary>
        /// <param name="model"><see cref="RehearsalPoint"/></param>
        [Authorize]
        [HttpPost]
        [Route("rehearsal")]
        public Task<IActionResult> Create(RehearsalPoint model) => CreateAsync(model);

        private async Task<IActionResult> CreateAsync(Profile model)
        {
            var userLogin = User.GetLogin();
            var canCreateResult = await _accessService.CanCreateProfileAsync(userLogin);

            if (!canCreateResult.CanCreate)
            {
                return BadRequest(new BaseResponse
                {
                    Success = false, 
                    Error = canCreateResult.Reason,
                });
            }

            var result = await _commandRepository.CreateAsync(new CommandParameter<Profile>
                {Parameter = model, UserLogin = userLogin});
            return Ok(new BaseResponse() {Success = result.Success, Error = result.Error});
        }

        /// <summary>
        /// Check can user add new profiles
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("profile/can-create")]
        public async Task<IActionResult> CanCreate()
        {
            var userLogin = User.GetLogin();
            var canCreateResult = await _accessService.CanCreateProfileAsync(userLogin);
            return Ok(canCreateResult);
        }

        ///<inheritdoc cref="UpdateAsync"/>
        [HttpPut]
        [Route("musician")]
        [Authorize]
        public Task<IActionResult> Update(Musician profile) => UpdateAsync(profile);
        
        ///<inheritdoc cref="UpdateAsync"/>
        [HttpPut]
        [Route("band")]
        [Authorize]
        public Task<IActionResult> Update(Band profile) => UpdateAsync(profile);
        
        ///<inheritdoc cref="UpdateAsync"/>
        [HttpPut]
        [Route("shop")]
        [Authorize]
        public Task<IActionResult> Update(Shop profile) => UpdateAsync(profile);
        
        ///<inheritdoc cref="UpdateAsync"/>
        [HttpPut]
        [Route("studio")]
        [Authorize]
        public Task<IActionResult> Update(Studio profile) => UpdateAsync(profile);
        
        ///<inheritdoc cref="UpdateAsync"/>
        [HttpPut]
        [Route("rehearsal")]
        [Authorize]
        public Task<IActionResult> Update(RehearsalPoint profile) => UpdateAsync(profile);

        /// <summary>
        /// Update a profile.
        /// You can't modify EntityType, Login, UserLogin, AvatarId. For avatar updating use "base/avatar" POST method.
        /// You can't modify band participants and musician's band. You should use specific API methods.
        /// </summary>
        /// <param name="model">Profile model which has only updated properties.</param>
        private async Task<IActionResult> UpdateAsync(Profile model)
        {
            var login = User.GetLogin();

            var parameter = new CommandParameter<Profile>
            {
                UserLogin = login,
                Parameter = model
            };

            var updateResult = await _commandRepository.UpdateAsync(parameter);
            return Ok(new BaseResponse {Success = updateResult.Success, Error = updateResult.Error});
        }

        /// <summary>
        /// Delete profile.
        /// </summary>
        /// <param name="login">Profile login</param>
        [HttpDelete]
        [Route("profile/{login}")]
        [Authorize]
        public async Task<IActionResult> Delete(string login)
        {
            var userLogin = User.GetLogin();

            var parameter = new CommandParameter<string>() {UserLogin = userLogin, Parameter = login};
            var deleteResult = await _commandRepository.DeleteAsync(parameter);

            return Ok(new BaseResponse() {Success = deleteResult.Success, Error = deleteResult.Error});
        }

        /// <summary>
        /// Profile's base information (specific for each profile type).
        /// </summary>
        /// <param name="login">Profile's login</param>
        [HttpGet]
        [Route("profile-preview/{login}")]
        public async Task<IActionResult> Get(string login)
        {
            var entity = await _queryRepository.GetAsync(login);
            if (entity == null)
            {
                return BadRequest("Invalid profile login.");
            }

            Request.SetProfileCorrectAvatarUrls(entity);
            return Ok(entity.ToSpecificPreviewModel());
        }

        /// <summary>
        /// Get profile avatar.
        /// (Byte array or base64 string)
        /// </summary>
        /// <param name="login">Profile's login</param>
        [HttpGet]
        [AllowAnonymous]
        [Route("avatar/{login}")]
        public async Task<HttpResponseMessage> GetImage(string login)
        {
            byte[] file = await _queryRepository.GetFileAsync(login);
            return ControllerMedia.MediaResponse(file);
        }

        /// <summary>
        /// Update profile's avatar.
        /// (Put null or empty byte array for avatar deleting)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(UpdateAvatarRequest request)
        {
            var login = User.GetLogin();

            var commandParameter = new CommandParameter<AvatarUpdateParameter>()
            {
                UserLogin = login,
                Parameter = new AvatarUpdateParameter() {Login = request.Login, Bytes = request.Photo}
            };

            ICommandResponse result = await _commandRepository.UpdateAvatarAsync(commandParameter);

            return Ok(new BaseResponse() {Success = result.Success, Error = result.Error});
        }


        /// <summary>
        /// Profile's logins which belongs to authorized user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IActionResult> GetUserEntitiesLogins()
        {
            var userLogin = User.GetLogin();
            var logins = await _queryRepository.GetUserEntitiesLoginsAsync(userLogin);
            return Ok(logins);
        }

        /// <summary>
        /// Check login for existance.
        /// </summary>
        /// <param name="login">Profile's login</param>
        /// <returns></returns>
        [HttpGet]
        [Route("check/{login}")]
        public async Task<IActionResult> CheckIfLoginExist(string login)
        {
            bool isExist = await _queryRepository.LoginExistsAsync(login);
            return Ok(isExist);
        }
    }
}