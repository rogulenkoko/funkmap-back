using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IBandRepository 
    {
        /// <summary>
        /// Чистит все зависимости от музыканта
        /// </summary>
        /// <param name="musician"></param>
        /// <param name=""></param>
        /// <param name="bandLogin">Если null, то чистит зависимости всех групп. Если указан, то для конкретной группы</param>
        /// <returns></returns>
        Task CleanMusiciansDependencies(Musician musician, string bandLogin = null);
    }
}
