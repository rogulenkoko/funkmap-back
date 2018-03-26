using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Tools;

namespace Funkmap.Data.Services
{
    public class EntityUpdateService : IEntityUpdateService
    {
        private readonly IBaseRepository _baseRepository;

        public EntityUpdateService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task CreateEntity(Profile model)
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

            await _baseRepository.CreateAsync(resultEntity);
        }

        public async Task UpdateEntity(Profile model)
        {
            BaseEntity resultEntity;
            var exictingEntity = await _baseRepository.GetAsync(model.Login);
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

            await _baseRepository.UpdateAsync(resultEntity);
        }
    }
}
