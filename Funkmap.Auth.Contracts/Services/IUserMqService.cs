using Funkmap.Auth.Contracts.Models;

namespace Funkmap.Auth.Contracts.Services
{
    public interface IUserMqService
    {
        UserLastVisitDateResponse GetLastVisitDate(UserLastVisitDateRequest request);

        UserUpdateLastVisitDateResponse UpdateLastVisitDate(UserUpdateLastVisitDateRequest request);
    }
}
