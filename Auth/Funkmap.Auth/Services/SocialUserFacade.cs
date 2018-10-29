using System.Collections.Generic;
using System.Linq;
using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Services
{
    public class SocialUserFacade
    {
        private readonly IEnumerable<ISocialUserService> _services;

        public SocialUserFacade(IEnumerable<ISocialUserService> services)
        {
            _services = services;
        }

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
