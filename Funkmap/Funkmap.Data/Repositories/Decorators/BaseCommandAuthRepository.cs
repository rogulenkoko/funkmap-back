using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Models;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories.Decorators
{
    public class BaseCommandAuthRepository : RepositoryBase<BaseEntity>, IBaseCommandRepository
    {
        private readonly IBaseCommandRepository _repository;

        public BaseCommandAuthRepository(IMongoCollection<BaseEntity> collection, IBaseCommandRepository repository) : base(collection)
        {
            _repository = repository;
        }

        public async Task<ICommandResponse> UpdateAsync(ICommandParameter<Profile> model)
        {
            var available = await IsAvailableAsync(model.UserLogin, model.Parameter.Login);
            if (!available) return BuildResponse(model.UserLogin, model.Parameter.Login);

            return await _repository.UpdateAsync(model);
        }

        public async Task<ICommandResponse> CreateAsync(ICommandParameter<Profile> model)
        {
            model.Parameter.UserLogin = model.UserLogin;
            return await _repository.CreateAsync(model);
        }

        public async Task<ICommandResponse> UpdateAvatarAsync(ICommandParameter<AvatarUpdateParameter> parameter, ImageProcessorOptions options = null)
        {
            var available = await IsAvailableAsync(parameter.UserLogin, parameter.Parameter.Login);
            if (!available) return BuildResponse(parameter.UserLogin, parameter.Parameter.Login);

            return await _repository.UpdateAvatarAsync(parameter, options);
        }

        public async Task<ICommandResponse<Profile>> DeleteAsync(ICommandParameter<string> parameter)
        {
            var available = await IsAvailableAsync(parameter.UserLogin, parameter.Parameter);
            if (!available)
            {
                return new CommandResponse<Profile>(false)
                {
                    Body = null,
                    Error = BuildResponse(parameter.UserLogin, parameter.Parameter).Error
                };
            }
            return await _repository.DeleteAsync(parameter);
        }

        public Task<ICommandResponse> UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            return _repository.UpdateFavoriteAsync(parameter);
        }

        public Task<ICommandResponse> UpdatePriorityAsync(string profileLogin)
        {
            return _repository.UpdatePriorityAsync(profileLogin);
        }

        private async Task<bool> IsAvailableAsync(string userLogin, string profileLogin)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, profileLogin) &
                         Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);

            var profile = await _collection.Find(filter).Project<BaseEntity>(projection).SingleOrDefaultAsync();

            return profile != null;
        }

        private CommandResponse BuildResponse(string userLogin, string profileLogin)
        {
            return new CommandResponse(false)
            {
                Error = $"Profile {profileLogin} is not availeble for {userLogin} user for updating."
            };
        }
    }
}
