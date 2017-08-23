using Funkmap.Auth.Contracts.Models;

namespace Funkmap.Auth.Contracts.Services
{
    public interface IUserMqService
    {
        UserUpdateLastVisitDateResponse UpdateLastVisitDate(UserUpdateLastVisitDateRequest request);
    }
}
