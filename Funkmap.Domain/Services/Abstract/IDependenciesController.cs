using System.Threading.Tasks;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IDependenciesController
    {
        void CleanDeletedDependencies(Profile deletedEntity);

        Task CleanDependenciesAsync(CleanDependenciesParameter parameter);

        Task CreateDependenciesAsync(UpdateBandMemberParameter request, bool needToAdd = true);
    }
}
