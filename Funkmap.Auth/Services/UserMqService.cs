using System;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Contracts.Services;
using ServiceStack.Messaging;

using IFunkmapAuthRepository = Funkmap.Auth.Data.Abstract.IAuthRepository;

namespace Funkmap.Module.Auth.Services
{
    public class UserMqService : IUserMqService
    {

        private readonly IMessageService _messageService;

        private readonly IFunkmapAuthRepository _authRepository;

        public UserMqService(IMessageService messageService, IFunkmapAuthRepository authRepository)
        {
            _messageService = messageService;
            _authRepository = authRepository;

            InitHandlers();
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<UserLastVisitDateRequest>(request => GetLastVisitDate(request?.GetBody()));
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
    }
}
