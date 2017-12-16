using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Services.Abstract;
using Funkmap.Tools;

namespace Funkmap.Services
{
    public class EntityUpdateService : IEntityUpdateService
    {
        private readonly IBaseRepository _baseRepository;

        public EntityUpdateService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task CreateEntity(BaseModel model)
        {
            BaseEntity resultEntity;

            switch (model.EntityType)
            {
                case EntityType.Musician:
                    resultEntity = (model as MusicianModel).ToMusicianEntity();
                    break;

                case EntityType.Band:
                    resultEntity = (model as BandModel).ToBandEntity();
                    break;

                case EntityType.Shop:
                    resultEntity = (model as ShopModel).ToShopEntity();
                    break;

                case EntityType.RehearsalPoint:
                    resultEntity = (model as RehearsalPointModel).ToRehearsalPointEntity();
                    break;

                case EntityType.Studio:
                    resultEntity = (model as StudioModel).ToStudioEntity();
                    break;
                default:
                    throw new ArgumentNullException(nameof(model.EntityType));
            }

            resultEntity.IsActive = true;
            resultEntity.CreationDate = DateTime.UtcNow;

            await _baseRepository.CreateAsync(resultEntity);
        }

        public async Task UpdateEntity(BaseModel model)
        {
            BaseEntity resultEntity;
            var exictingEntity = await _baseRepository.GetAsync(model.Login);
            if(exictingEntity == null) throw new InvalidOperationException("Entity doesn't exist");

            switch (model.EntityType)
            {
                case EntityType.Musician:
                    var newMusicianEntity = (model as MusicianModel).ToMusicianEntity();
                    resultEntity = (exictingEntity as MusicianEntity).FillEntity(newMusicianEntity);
                    break;

                case EntityType.Band:
                    var newBandEntity = (model as BandModel).ToBandEntity();
                    resultEntity = (exictingEntity as BandEntity).FillEntity(newBandEntity);
                    break;

                case EntityType.Shop:
                    var newShopEntity = (model as ShopModel).ToShopEntity();
                    resultEntity = (exictingEntity as ShopEntity).FillEntity(newShopEntity);
                    break;

                case EntityType.RehearsalPoint:
                    var newRehearsalEntity = (model as RehearsalPointModel).ToRehearsalPointEntity();
                    resultEntity = (exictingEntity as RehearsalPointEntity).FillEntity(newRehearsalEntity);
                    break;

                case EntityType.Studio:
                    var newStudioEntity = (model as StudioModel).ToStudioEntity();
                    resultEntity = (exictingEntity as StudioEntity).FillEntity(newStudioEntity);
                    break;
                default:
                    throw new ArgumentNullException(nameof(model.EntityType));
            }

            await _baseRepository.UpdateAsync(resultEntity);
        }
    }
}
