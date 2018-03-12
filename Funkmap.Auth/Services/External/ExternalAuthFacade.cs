using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth.Services.External.Abstract;

namespace Funkmap.Module.Auth.Services.External
{
    public class ExternalAuthFacade
    {
        private readonly IEnumerable<IExternalAuthService> _externalValidationServices;

        public ExternalAuthFacade(IEnumerable<IExternalAuthService> externalValidationServices)
        {
            _externalValidationServices = externalValidationServices;
        }

        public async Task<UserEntity> BuildUserAsync(string token, AuthProviderType providerType)
        {
            var validationService = _externalValidationServices.SingleOrDefault(x => x.ProviderType == providerType);

            if (validationService == null)
            {
                return null;
            }

            UserEntity userEntity = await validationService.BuildUserAsync(token);

            return userEntity;
        }
    }
}
