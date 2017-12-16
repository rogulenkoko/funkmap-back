using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IMusicianRepository : IMongoRepository<MusicianEntity>
    {
        /// <summary>
        /// Чистит все зависимости от группы
        /// </summary>
        /// <param name="band"></param>
        /// <param name="musicianLogin">Если null, то чистит зависимости всех музыкантов. Если указан, то для конкретного музыканта</param>
        /// <returns></returns>
        Task CleanBandDependencies(BandEntity band, string musicianLogin = null);
    }
}
