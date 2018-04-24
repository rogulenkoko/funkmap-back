using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IBandRepository 
    {
        /// <summary>
        /// Чистит все зависимости от музыканта
        /// </summary>
        /// <returns></returns>
        Task ProcessMusicianDependenciesAsync(Musician musician, Musician updatedMusician = null);
    }
}
