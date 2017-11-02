using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBandRepository : IMongoRepository<BandEntity>
    {
        /// <summary>
        /// Чистит все зависимости от музыканта
        /// </summary>
        /// <param name="musician"></param>
        /// <param name=""></param>
        /// <param name="bandLogin">Если null, то чистит зависимости всех групп. Если указан, то для конкретной группы</param>
        /// <returns></returns>
        Task CleanMusiciansDependencies(MusicianEntity musician, string bandLogin = null);
    }
}
