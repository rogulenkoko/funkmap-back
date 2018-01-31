using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Logger;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Module.Auth.Abstract;
using Funkmap.Module.Auth.Models;
using Funkmap.Module.Auth.Notifications;

namespace Funkmap.Module.Auth.Services
{
    
    public class RegistrationContextManager : IRegistrationContextManager, IRestoreContextManager
    {
        //todo timer
        private readonly IAuthRepository _authRepository;

        private readonly IExternalNotificationService _externalNotificationService;

        private readonly ConcurrentDictionary<string, RegistrationContext> _contexts;

        private readonly ConcurrentDictionary<string, RestoreContext> _restoreContexts;

        private readonly IFunkmapLogger<RegistrationContextManager> _logger;

        public RegistrationContextManager(IAuthRepository authRepository, IExternalNotificationService externalNotificationService, 
            IFunkmapLogger<RegistrationContextManager> logger)
        {
            _authRepository = authRepository;
            _externalNotificationService = externalNotificationService;
            _logger = logger;
            _contexts = new ConcurrentDictionary<string, RegistrationContext>();
            _restoreContexts = new ConcurrentDictionary<string, RestoreContext>();
        }

        #region IRegistrationContextManager

        public async Task<bool> ValidateLogin(string login)
        {
            var isExist = await _authRepository.CheckIfExist(login);

            if (isExist)
            {
                return false;
            }

            if (_contexts.Any(x => x.Value.User.Login == login))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> TryCreateContextAsync(RegistrationRequest creds)
        {
            var hash = CryptoProvider.ComputeHash($"{creds.Email}_{creds.Login}");

            if (_contexts.ContainsKey(hash))
            {
                return false;
            }

            var isLoginValid = await ValidateLogin(creds.Login);

            if (!isLoginValid)
            {
                return false;
            }


            var password = CryptoProvider.ComputeHash(creds.Password);

            var user = new UserEntity() { Login = creds.Login, Password = password, Name = creds.Name, Email = creds.Email, Locale = creds.Locale};


            var bookedContextEmails = _contexts.Where(x => !String.IsNullOrEmpty(x.Value.User.Email)).Select(x => x.Value.User.Email);

            var bookedDbEmails = await _authRepository.GetBookedEmailsAsync();

            var allBookedEmails = bookedDbEmails.Concat(bookedContextEmails);

            if (allBookedEmails.Any(x => x == creds.Email))
            {
                return false;
            }

            var context = new RegistrationContext(user)
            {
                Code = new Random().Next(100000, 999999).ToString()
            };
            

            var notification = new ConfirmationNotification(context.User.Email, context.User.Name, context.Code);
            var sendResult = await _externalNotificationService.TrySendNotificationAsync(notification);

            if (!sendResult)
            {
                return false;
            }
            
            _contexts.TryAdd(hash, context);

            return true;
        }

        public async Task<bool> TryConfirmAsync(string login, string email, string code)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(code))
            {
                throw new ArgumentException("empty login or code");
            }

            RegistrationContext context;

            var hash = CryptoProvider.ComputeHash($"{email}_{login}");

            _contexts.TryGetValue(hash, out context);

            if (context == null) return false;

            if (context.Code != code) return false;

            context.User.LastVisitDateUtc = DateTime.UtcNow;


            try
            {
                await _authRepository.CreateAsync(context.User);

                RegistrationContext deletedContext;
                _contexts.TryRemove(hash, out deletedContext);

                _logger.Info($"{context.User.Login} has registered");
            }
            catch (Exception e)
            {
                _logger.Error(e);
                _logger.Info($"{context.User.Login}'s registration failed");
                return false;
            }
            
            return true;
        }

        #endregion

        #region IRestoreContextManager

        public async Task<bool> TryCreateRestoreContextAsync(string loginOrEmail)
        {
            UserEntity user = await _authRepository.GetUserByEmailOrLogin(loginOrEmail);

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

            var context = new RestoreContext() {Email = user.Email,Code = code };

            _restoreContexts.TryAdd(user.Email, context);

            return true;
        }

        public async Task<bool> TryConfirmRestoreAsync(string loginOrEmail, string code, string newPassword)
        {
            UserEntity user = await _authRepository.GetUserByEmailOrLogin(loginOrEmail);


            if (user == null)
            {
                return false;
            }

            RestoreContext context;
            _restoreContexts.TryGetValue(user.Email, out context);

            if (context == null)
            {
                return false;
            }

            if (context.Code != code)
            {
                return false;
            }

            var newPasswordHash = CryptoProvider.ComputeHash(newPassword);

            user.Password = newPasswordHash;

            try
            {
                await _authRepository.UpdateAsync(user);
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

