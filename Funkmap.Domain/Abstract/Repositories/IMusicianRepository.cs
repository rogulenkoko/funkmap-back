using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IMusicianRepository
    {
        /// <summary>
        /// Чистит все зависимости от группы
        /// </summary>
        /// <param name="band"></param>
        /// <param name="musicianLogin">Если null, то чистит зависимости всех музыкантов. Если указан, то для конкретного музыканта</param>
        /// <returns></returns>
        Task CleanBandDependencies(Band band, string musicianLogin = null);
    }
}
