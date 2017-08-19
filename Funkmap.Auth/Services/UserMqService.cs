using System;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Contracts.Services;
using Funkmap.Common.RedisMq;
using ServiceStack.Messaging;

using IFunkmapAuthRepository = Funkmap.Auth.Data.Abstract.IAuthRepository;

namespace Funkmap.Module.Auth.Services
{
    public class UserMqService : IUserMqService, IRedisMqService
    {

        private readonly IMessageService _messageService;

        private readonly IFunkmapAuthRepository _authRepository;

        public UserMqService(IMessageService messageService, IFunkmapAuthRepository authRepository)
        {
            _messageService = messageService;
            _authRepository = authRepository;
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<UserLastVisitDateRequest>(request => GetLastVisitDate(request?.GetBody()));
            _messageService.RegisterHandler<UserUpdateLastVisitDateRequest>(request => UpdateLastVisitDate(request?.GetBody()));
        }

        public UserLastVisitDateResponse GetLastVisitDate(UserLastVisitDateRequest request)
        {
            if(request == null) throw new ArgumentNullException();

            var date = _authRepository.GetLastVisitDate(request.Login).GetAwaiter().GetResult();

            return new UserLastVisitDateResponse()
            {
                LastVisitDateUtc = date
            };
        }

        public UserUpdateLastVisitDateResponse UpdateLastVisitDate(UserUpdateLastVisitDateRequest request)
        {
            if (request == null) throw new ArgumentNullException();
            var response = new UserUpdateLastVisitDateResponse();

            try
            {
                _authRepository.UpdateLastVisitDateAsync(request.Login, DateTime.UtcNow).GetAwaiter().GetResult();
                response.Success = true;
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return response;
            }
            
        }
    }
}
