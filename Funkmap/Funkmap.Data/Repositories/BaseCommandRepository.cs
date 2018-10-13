using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Models;
using Funkmap.Common.Owin.Tools;
using Funkmap.Cqrs.Abstract;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Services.Update;
using Funkmap.Data.Tools;
using Funkmap.Domain;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Events;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BaseCommandRepository : RepositoryBase<BaseEntity>, IBaseCommandRepository
    {
        private readonly IFileStorage _fileStorage;
        private readonly ICollection<IUpdateDefenitionBuilder> _specificUpdateDefenitionBuilders;
        private readonly IEventBus _eventBus;

        public BaseCommandRepository(IMongoCollection<BaseEntity> collection,
            [KeyFilter(CollectionNameProvider.StorageName)]IFileStorage fileStorage,
            ICollection<IUpdateDefenitionBuilder> specificUpdateDefenitionBuilders,
            IEventBus eventBus) : base(collection)
        {
            _fileStorage = fileStorage;
            _specificUpdateDefenitionBuilders = specificUpdateDefenitionBuilders;
            _eventBus = eventBus;
        }

        public async Task<ICommandResponse> CreateAsync(ICommandParameter<Profile> parameter)
        {
            var model = parameter.Parameter;


            if (String.IsNullOrEmpty(model.Login))
            {
                return new CommandResponse(false) {Error = "Login can not be null or empty."};
            }

            if (String.IsNullOrEmpty(model.Name))
            {
                return new CommandResponse(false) { Error = "Name can not be null or empty." };
            }

            if (model.Location == null)
            {
                return new CommandResponse(false) {Error = "Location can not be null."};
            }

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
                    return new CommandResponse(false) { Error = "Invalid profile type." };
            }

            resultEntity.IsActive = true;
            resultEntity.CreationDate = DateTime.UtcNow;

            await _collection.InsertOneAsync(resultEntity);

            return new CommandResponse(true);
        }

        public async Task<ICommandResponse> UpdateAsync(ICommandParameter<Profile> parameter)
        {
            var model = parameter.Parameter;

            if (String.IsNullOrEmpty(model.Login))
            {
                return new CommandResponse(false) { Error = "You should set Login property for entity update." };
            }

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, model.Login);

            var entityTypeProjection = Builders<BaseEntity>.Projection.Include(x => x.EntityType);

            var entityWitType = await _collection.Find(filter)
                .Project<BaseEntity>(entityTypeProjection)
                .SingleOrDefaultAsync();

            if (entityWitType == null)
            {
                return new CommandResponse(false) { Error = $"Profile with login {model.Login} doesn't exist." };
            }

            model.EntityType = entityWitType.EntityType;

            var builder = new FunkmapUpdateDefinitionBuilder(_specificUpdateDefenitionBuilders);
            var update = builder.BuildBaseUpdateDefinition(model).BuildSpecificUpdateDefinition();

            if (update == null)
            {
                return new CommandResponse(false) { Error = "There are not updated properties." };
            }
            
            var profile = await _collection.FindOneAndUpdateAsync(filter, update);

            var updatedProfile = await _collection.Find(filter).SingleOrDefaultAsync();

            var updateEvent = new ProfileUpdatedEvent
            {
                Profile = profile.ToSpecificProfile(),
                UpdatedProfile = updatedProfile.ToSpecificProfile()
            };

            await _eventBus.PublishAsync(updateEvent);

            return new CommandResponse(profile != null);
        }

        public async Task<ICommandResponse<Profile>> DeleteAsync(ICommandParameter<string> parameter)
        {
            var login = parameter.Parameter;

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, login);
            var entity = await _collection.FindOneAndDeleteAsync(filter);

            var profile = entity.ToSpecificProfile();

            await _eventBus.PublishAsync(new ProfileDeletedEvent {Profile = profile});

            return new CommandResponse<Profile>(true) { Body = profile };
        }

        public async Task<ICommandResponse> UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            if (String.IsNullOrEmpty(parameter?.ProfileLogin) || String.IsNullOrEmpty(parameter.UserLogin))
            {
                return new CommandResponse(false) { Error = "Profile login and user login can not be null." };
            }

            FilterDefinition<BaseEntity> filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, parameter.ProfileLogin);
            UpdateDefinition<BaseEntity> update;

            if (parameter.IsFavorite)
            {
                filter = filter & Builders<BaseEntity>.Filter.AnyNe(x => x.FavoriteFor, parameter.UserLogin);
                update = Builders<BaseEntity>.Update.Push(x => x.FavoriteFor, parameter.UserLogin);
            }
            else
            {
                filter = filter & Builders<BaseEntity>.Filter.AnyEq(x => x.FavoriteFor, parameter.UserLogin);
                update = Builders<BaseEntity>.Update.Pull(x => x.FavoriteFor, parameter.UserLogin);
            }

            var updateResult = await _collection.UpdateOneAsync(filter, update);

            return new CommandResponse(updateResult.ModifiedCount == 1);
        }

        public async Task<ICommandResponse> UpdateAvatarAsync(ICommandParameter<AvatarUpdateParameter> parameter, ImageProcessorOptions options = null)
        {

            if (options == null)
            {
                options = new ImageProcessorOptions();
            }

            string login = parameter.Parameter.Login;
            byte[] imageBytes = parameter.Parameter.Bytes;

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, login);

            String photoId;
            String photoMiniUrl;

            if (imageBytes == null || imageBytes.Length == 0)
            {
                photoId = String.Empty;
                photoMiniUrl = String.Empty;
            }
            else
            {
                var fileName = ImageNameBuilder.BuildAvatarName(login);
                var imageBytesNormal = FunkmapImageProcessor.MinifyImage(imageBytes, options.Size);
                photoId = await _fileStorage.UploadFromBytesAsync(fileName, imageBytesNormal);

                var fileMiniName = ImageNameBuilder.BuildAvatarMiniName(login);
                var imageMiniBytes = FunkmapImageProcessor.MinifyImage(imageBytes, options.MiniSize);
                photoMiniUrl = await _fileStorage.UploadFromBytesAsync(fileMiniName, imageMiniBytes);
            }

            var update = Builders<BaseEntity>.Update.Set(x => x.AvatarUrl, photoId).Set(x => x.AvatarMiniUrl, photoMiniUrl);

            var result = await _collection.UpdateOneAsync(filter, update);
            return new CommandResponse(result.ModifiedCount == 1);
        }
    }
}
