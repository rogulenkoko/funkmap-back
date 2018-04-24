using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IMusicianRepository
    {
        /// <summary>
        /// Process all Mongo band-musician dependencies
        /// </summary>
        /// <returns></returns>
        Task ProcessBandDependenciesAsync(Band band, Band updatedBand = null);
    }
}
