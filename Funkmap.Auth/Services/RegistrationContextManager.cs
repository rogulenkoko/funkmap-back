using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Logger;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Module.Auth.Models;
using Funkmap.Module.Auth.Notifications;

namespace Funkmap.Module.Auth.Services
{
    public interface IRegistrationContextManager
    {
        Task<bool> TryCreateContextAsync(RegistrationRequest creds);

        Task<bool> TrySendCodeAsync(string login, string email);

        Task<bool> TryConfirmAsync(string login, string code);

    }
    public class RegistrationContextManager : IRegistrationContextManager
    {
        //todo timer
        private readonly IAuthRepository _authRepository;

        private readonly IExternalNotificationService _externalNotificationService;

        private readonly ConcurrentBag<RegistrationContext> _contexts;

        private readonly IFunkmapLogger<RegistrationContextManager> _logger;

        public RegistrationContextManager(IAuthRepository authRepository, IExternalNotificationService externalNotificationService, 
            IFunkmapLogger<RegistrationContextManager> logger)
        {
            _authRepository = authRepository;
            _externalNotificationService = externalNotificationService;
            _logger = logger;
            _contexts = new ConcurrentBag<RegistrationContext>();
        }

        public async Task<bool> TryCreateContextAsync(RegistrationRequest creds)
        {
            var isExist = await _authRepository.CheckIfExist(creds.Login);

            if (isExist)
            {
                return false;
            }

            if (_contexts.Any(x => x.Login == creds.Login))
            {
                return false;
            }

            

            creds.Password = CryptoProvider.ComputeHash(creds.Password);

            var user = new UserEntity() { Login = creds.Login, Password = creds.Password, Name = creds.Name };

            var context = new RegistrationContext(user);
            
            _contexts.Add(context);

            return true;
        }

        public async Task<bool> TrySendCodeAsync(string login, string email)
        {

            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("empty login or email");
            }

            if (!_contexts.Select(x=>x.Login).Contains(login))
            {
                return false;
            }

            var bookedContextEmails = _contexts.Where(x => !String.IsNullOrEmpty(x.Email)).Select(x => x.Email);

            var bookedDbEmails = await _authRepository.GetBookedEmailsAsync();

            var allBookedEmails = bookedDbEmails.Concat(bookedContextEmails);

            if (allBookedEmails.Any(x=> x == email))
            {
                return false;
            }

            var context = _contexts.SingleOrDefault(x => x.Login == login);
            if (context == null)
            {
                return false;
            }


            context.Code = new Random().Next(100000, 999999).ToString();
            
            context.Email = email;

            var notification = new ConfirmationNotification(context.User.Email, context.User.Name, context.Code);
            var sendResult = await _externalNotificationService.TrySendNotificationAsync(notification);

            return sendResult;

        }

        public async Task<bool> TryConfirmAsync(string login, string code)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(code))
            {
                throw new ArgumentException("empty login or code");
            }

            var context = _contexts.SingleOrDefault(x => x.Login == login);
            if (context == null) return false;

            if (context.Code != code) return false;

            context.User.LastVisitDateUtc = DateTime.UtcNow;


            try
            {
                await _authRepository.CreateAsync(context.User);
                _logger.Info($"{context.Login} has registered");
            }
            catch (Exception e)
            {
                _logger.Error(e);
                _logger.Info($"{context.Login}'s registration failed");
                return false;
            }
            
            return true;
        }
    }
}
