using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Models;
using Funkmap.Models.Requests;

namespace Funkmap.Services.Abstract
{
    public interface IDependenciesController
    {
        void CleanDeletedDependencies(BaseEntity deletedEntity);

        Task CleanDependencies(CleanDependenciesParameter parameter);

        Task CreateDependencies(UpdateBandMembersRequest request, bool needToAdd = true);
    }
}
