using System.Threading.Tasks;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Services.Abstract;

namespace Funkmap.Domain.Services
{
    public class AccessService : IAccessService
    {
        private readonly IProAccountRepository _proAccountRepository;
        private readonly IBaseQueryRepository _queryRepository;

        private const int MaxFreeProfilesCount = 1;
        private const int MaxProfilesCount = 5;

        public AccessService(IProAccountRepository proAccountRepository,
                            IBaseQueryRepository queryRepository)
        {
            _proAccountRepository = proAccountRepository;
            _queryRepository = queryRepository;
        }

        public async Task<CanCreateProfileResult> CanCreateProfileAsync(string userLogin)
        {
            var proAccount = await _proAccountRepository.GetAsync(userLogin);

            if (proAccount == null)
            {
                var userEntities = await _queryRepository.GetUserEntitiesLoginsAsync(userLogin);

                if (userEntities.Count >= MaxFreeProfilesCount)
                {
                    return new CanCreateProfileResult(false, $"You can create up to {MaxFreeProfilesCount} profiles.");
                }
            }

            return new CanCreateProfileResult(true);
        }
    }
}
