using System.Collections.Generic;
using System.Linq;
using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Services
{
    /// <summary>
    /// Facade for external authorization services
    /// </summary>
    public class SocialUserFacade
    {
        private readonly IEnumerable<ISocialUserService> _services;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="services"><see cref="ISocialUserService"/></param>
        public SocialUserFacade(IEnumerable<ISocialUserService> services)
        {
            _services = services;
        }

        /// <summary>
        /// Get external authorization service user
        /// </summary>
        /// <param name="token">External service authorization token</param>
        /// <param name="provider">External authorization provider name</param>
        /// <param name="user"><see cref="User"/></param>
        public bool TryGetSocialUser(string token, string provider, out User user)
        {
            var service = _services.SingleOrDefault(x => x.Provider == provider);
            if (service == null)
            {
                user = null;
                return false;
            }

            return service.TryGetUser(token, out user);
        }
    }
}
