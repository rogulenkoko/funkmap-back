using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Tools;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class UpdateRepository : IUpdateRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;

        public UpdateRepository(IMongoCollection<BaseEntity> collection)
        {
            _collection = collection;
        }

        public async Task Create(Profile model)
        {
            BaseEntity resultEntity;

            switch (model.EntityType)
            {
                case EntityType.Musician:
                    resultEntity = (model as Musician).ToMusicianEntity();
                    break;

                case EntityType.Band:
                    resultEntity = (model as Band).ToBandEntity();
                    break;

                case EntityType.Shop:
                    resultEntity = (model as Shop).ToShopEntity();
                    break;

                case EntityType.RehearsalPoint:
                    resultEntity = (model as RehearsalPoint).ToRehearsalPointEntity();
                    break;

                case EntityType.Studio:
                    resultEntity = (model as Studio).ToStudioEntity();
                    break;
                default:
                    throw new ArgumentNullException(nameof(model.EntityType));
            }

            resultEntity.IsActive = true;
            resultEntity.CreationDate = DateTime.UtcNow;

            await _collection.InsertOneAsync(resultEntity);
        }

        public async Task UpdateAsync(Profile model)
        {
            BaseEntity resultEntity;

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, model.Login);

            var exictingEntity = await _collection.Find(filter).SingleOrDefaultAsync();
            if(exictingEntity == null) throw new InvalidOperationException("Entity doesn't exist");

            switch (model.EntityType)
            {
                case EntityType.Musician:
                    var newMusicianEntity = (model as Musician).ToMusicianEntity();
                    resultEntity = (exictingEntity as MusicianEntity).FillEntity(newMusicianEntity);
                    break;

                case EntityType.Band:
                    var newBandEntity = (model as Band).ToBandEntity();
                    resultEntity = (exictingEntity as BandEntity).FillEntity(newBandEntity);
                    break;

                case EntityType.Shop:
                    var newShopEntity = (model as Shop).ToShopEntity();
                    resultEntity = (exictingEntity as ShopEntity).FillEntity(newShopEntity);
                    break;

                case EntityType.RehearsalPoint:
                    var newRehearsalEntity = (model as RehearsalPoint).ToRehearsalPointEntity();
                    resultEntity = (exictingEntity as RehearsalPointEntity).FillEntity(newRehearsalEntity);
                    break;

                case EntityType.Studio:
                    var newStudioEntity = (model as Studio).ToStudioEntity();
                    resultEntity = (exictingEntity as StudioEntity).FillEntity(newStudioEntity);
                    break;
                default:
                    throw new ArgumentNullException(nameof(model.EntityType));
            }

            await _collection.ReplaceOneAsync(filter, resultEntity);
        }
    }
}
