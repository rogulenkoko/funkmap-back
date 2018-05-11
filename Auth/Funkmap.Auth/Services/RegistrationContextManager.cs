using System;
using System.Threading.Tasks;
using Funkmap.Auth.Abstract;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Domain.Models;
using Funkmap.Auth.Models;
using Funkmap.Auth.Notifications;
using Funkmap.Common.Abstract;
using Funkmap.Common.Models;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Logger;

namespace Funkmap.Auth.Services
{

    public class RegistrationContextManager : IRegistrationContextManager, IRestoreContextManager
    {
        private readonly IAuthRepository _authRepository;

        private readonly IExternalNotificationService _externalNotificationService;

        private readonly IStorage _storage;

        private readonly IConfirmationCodeGenerator _codeGenerator;

        private readonly IFunkmapLogger<RegistrationContextManager> _logger;

        private readonly TimeSpan _sessionTimeSpan = TimeSpan.FromMinutes(15);

        private const string RegistrationContextsKey = "registration_contexts";
        private const string RestoreContextsKey = "restore_contexts";

        public RegistrationContextManager(IAuthRepository authRepository, IExternalNotificationService externalNotificationService,
            IStorage storage,
            IConfirmationCodeGenerator codeGenerator,
            IFunkmapLogger<RegistrationContextManager> logger)
        {
            _authRepository = authRepository;
            _externalNotificationService = externalNotificationService;
            _logger = logger;
            _storage = storage;
            _codeGenerator = codeGenerator;
        }

        #region IRegistrationContextManager

        public async Task<BaseResponse> TryCreateContextAsync(RegistrationRequest creds)
        {
            var hash = CryptoProvider.ComputeHash($"{creds.Email}_{creds.Login}");
            var key = $"{RegistrationContextsKey}_{hash}";

            var existingContext = await _storage.GetAsync<RegistrationContext>(key);

            if (existingContext != null)
            {
                return new BaseResponse() {Success = false, Error = "Registration context is already exists." };
            }

            var bookedEmails = await _authRepository.GetBookedEmailsAsync();

            if (bookedEmails.Contains(creds.Email))
            {
                return new BaseResponse() {Success = false, Error = "User with such email already exists."};
            }

            var user = new User { Login = creds.Login, Name = creds.Name, Email = creds.Email, Locale = creds.Locale };

            var context = new RegistrationContext(user, creds.Password)
            {
                Code = _codeGenerator.Generate()
            };

            var notification = new ConfirmationNotification(context.User.Email, context.User.Name, context.Code);
            var sendResult = await _externalNotificationService.TrySendNotificationAsync(notification);

            if (!sendResult)
            {
                return new BaseResponse() {Success = false, Error = "Can't send notification. Check your email." };
            }

            try
            {
                await _storage.SetAsync(key, context, _sessionTimeSpan);
                return new BaseResponse(){Success = true };
            }
            catch (Exception e)
            {
                return new BaseResponse() { Success = false, Error = e.Message };
            }
        }

        public async Task<BaseResponse> TryConfirmAsync(string login, string email, string code)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(code))
            {
                return new BaseResponse() { Success = false, Error = "Invalid login or confirmation code." };
            }

            var bookedEmails = await _authRepository.GetBookedEmailsAsync();

            if (bookedEmails.Contains(email))
            {
                return new BaseResponse() {Success = false, Error = "User with such email already exists." };
            }

            var hash = CryptoProvider.ComputeHash($"{email}_{login}");
            var key = $"{RegistrationContextsKey}_{hash}";

            RegistrationContext context = await _storage.GetAsync<RegistrationContext>(key);

            if (context == null) return new BaseResponse() {Success = false, Error = "There is no registration context. You should ask for confirmation code." };

            if (context.Code != code) return new BaseResponse() {Success = false, Error = "The confirmation code is invalid." };

            context.User.LastVisitDateUtc = DateTime.UtcNow;


            var response = await _authRepository.TryCreateAsync(context.User, context.Password);

            if (response.Success)
            {
                await _storage.RemoveAsync(key);

                _logger.Info($"{context.User.Login} has registered");
            }

            return response;
        }

        #endregion

        #region IRestoreContextManager

        public async Task<bool> TryCreateRestoreContextAsync(string loginOrEmail)
        {
            User user = await _authRepository.GetAsync(loginOrEmail);

            if (user == null)
            {
                return false;
            }

            var code = new Random().Next(100000, 999999).ToString();

            var notification = new PasswordRecoverNotification(user.Email, user.Name, code);
            var sendResult = await _externalNotificationService.TrySendNotificationAsync(notification);

            if (!sendResult)
            {
                return false;
            }

            var context = new RestoreContext() { Email = user.Email, Code = code };

            await _storage.SetAsync($"{RestoreContextsKey}_{user.Email}", context, _sessionTimeSpan);

            return true;
        }

        public async Task<bool> TryConfirmRestoreAsync(string loginOrEmail, string code, string newPassword)
        {
            User user = await _authRepository.GetAsync(loginOrEmail);

            if (user == null)
            {
                return false;
            }

            RestoreContext context = await _storage.GetAsync<RestoreContext>($"{RestoreContextsKey}_{user.Email}");

            if (context == null)
            {
                return false;
            }

            if (context.Code != code)
            {
                return false;
            }

            var newPasswordHash = CryptoProvider.ComputeHash(newPassword);

            try
            {
                await _authRepository.UpdatePasswordAsync(user.Login, newPasswordHash);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

            return true;

        }

        #endregion
    }
}

