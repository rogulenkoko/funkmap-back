using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Mappers;
using Funkmap.Models;
using Funkmap.Tools;

namespace Funkmap.Services
{
    public interface IEntityUpdateService
    {
        Task UpdateEntity(BaseModel model);
    }

    public class EntityUpdateService : IEntityUpdateService
    {
        private readonly IBaseRepository _baseRepository;

        public EntityUpdateService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task UpdateEntity(BaseModel model)
        {
            BaseEntity resultEntity;
            var existingEntityCollection = await _baseRepository.GetSpecificNavigationAsync(new[] {model.Login});
            var exictingEntity = existingEntityCollection.SingleOrDefault();
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
