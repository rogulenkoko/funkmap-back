using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain.Models;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Services.Update
{
    public class FunkmapUpdateDefinitionBuilder
    {
        private readonly ICollection<IUpdateDefinitionBuilder> _specificUpdateDefenitionBuilders;


        private Profile _profile;

        private UpdateDefinition<BaseEntity> _updateDefinition;

        public FunkmapUpdateDefinitionBuilder(ICollection<IUpdateDefinitionBuilder> specificUpdateDefenitionBuilders)
        {
            _specificUpdateDefenitionBuilders = specificUpdateDefenitionBuilders;
        }

        public FunkmapUpdateDefinitionBuilder BuildBaseUpdateDefinition(Profile model)
        {
            var update = Builders<BaseEntity>.Update;
            
            var updateDefenitions = new List<UpdateDefinition<BaseEntity>>();

            if (model.Location != null)
            {
                updateDefenitions.Add(update.Set(x => x.Location,
                    new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                        new GeoJson2DGeographicCoordinates(model.Location.Longitude, model.Location.Latitude))));
            }

            if (model.Name != null)
            {
                updateDefenitions.Add(update.Set(x => x.Name, model.Name));
            }

            if (model.Address != null)
            {
                updateDefenitions.Add(update.Set(x => x.Address, model.Address));
            }

            if (model.Description != null)
            {
                updateDefenitions.Add(update.Set(x => x.Description, model.Description));
            }

            if (model.FacebookLink != null)
            {
                updateDefenitions.Add(update.Set(x => x.FacebookLink, model.FacebookLink));
            }

            if (model.SoundCloudLink != null)
            {
                updateDefenitions.Add(update.Set(x => x.SoundCloudLink, model.SoundCloudLink));
            }

            if (model.VkLink != null)
            {
                updateDefenitions.Add(update.Set(x=>x.VkLink, model.VkLink));
            }

            if (model.YoutubeLink != null)
            {
                updateDefenitions.Add(update.Set(x=>x.YouTubeLink, model.YoutubeLink));
            }

            if (model.IsActive.HasValue)
            {
                updateDefenitions.Add(update.Set(x=>x.IsActive, model.IsActive));
            }

            if (model.SoundCloudTracks != null)
            {
                var tracksEntity = model.SoundCloudTracks.GroupBy(x=>x.Id).Select(x=>x.First()).ToList().ToEntities();
                updateDefenitions.Add(update.Set(x => x.SoundCloudTracks, tracksEntity));
            }

            if (model.VideoInfos != null)
            {
                var videosEntity = model.VideoInfos.GroupBy(x => x.Id).Select(x => x.First()).ToList().ToEntities();
                updateDefenitions.Add(update.Set(x => x.VideoInfos, videosEntity));
            }

            _profile = model;

            _updateDefinition = updateDefenitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefenitions);

            return this;
        }

        public UpdateDefinition<BaseEntity> BuildSpecificUpdateDefinition()
        {
            var specificBuilder = _specificUpdateDefenitionBuilders.SingleOrDefault(x => x.EntityType == _profile.EntityType);

            if (specificBuilder == null)
            {
                return _updateDefinition;
            }

            var specificUpdateDefinition = specificBuilder.Build(_profile);

            if (specificUpdateDefinition == null && _updateDefinition == null) return null;

            var resultList = new List<UpdateDefinition<BaseEntity>>();

            if (specificUpdateDefinition != null)
            {
                resultList.Add(specificUpdateDefinition);
            }

            if (_updateDefinition != null)
            {
                resultList.Add(_updateDefinition);
            }
            
            return Builders<BaseEntity>.Update.Combine(resultList);
        }
    }
}
