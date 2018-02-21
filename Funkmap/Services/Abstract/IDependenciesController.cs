using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Models;
using Funkmap.Models.Requests;

namespace Funkmap.Services.Abstract
{
    public interface IDependenciesController
    {
        void CleanDeletedDependencies(BaseEntity deletedEntity);

        Task CleanDependenciesAsync(CleanDependenciesParameter parameter);

        Task CreateDependenciesAsync(UpdateBandMemberRequest request, bool needToAdd = true);
    }
}
