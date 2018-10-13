using Funkmap.Auth.Contracts;
using Funkmap.Auth.Domain.Models;

namespace Funkmap.Auth.Services
{
    public interface ISocialUserService
    {
        string Provider { get; }
        bool TryGetUser(string token, out User user);
    }
}