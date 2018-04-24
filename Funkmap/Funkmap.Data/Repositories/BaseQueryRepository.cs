using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BaseQueryRepository : RepositoryBase<BaseEntity>, IBaseQueryRepository
    {
        private readonly IFilterFactory _filterFactory;
        private readonly IFileStorage _fileStorage;

        public BaseQueryRepository(IMongoCollection<BaseEntity> collection,
                              [KeyFilter(CollectionNameProvider.StorageName)]IFileStorage fileStorage,
                              IFilterFactory filterFactory) : base(collection)
        {
            _filterFactory = filterFactory;
            _fileStorage = fileStorage;
        }

        public async Task<Profile> GetAsync(string login)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, login);
            var entity = await _collection.Find(filter).SingleOrDefaultAsync();
            return entity.ToSpecificProfile();
        }

        public async Task<T> GetAsync<T>(string login) where T : Profile
        {
            var profile = await GetAsync(login);
            return profile as T;
        }

        public async Task<List<Marker>> GetNearestMarkersAsync(LocationParameter parameter)
        {
            //db.bases.find({ loc: { $nearSphere: [50, 30], $minDistance: 0, $maxDistance: 1 } }).limit(20)

            if (parameter == null) throw new ArgumentException("Location parameter can not be null");

            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true) & BuildGeoFilter(parameter);

            List<BaseEntity> result = await _collection.Find(filter).Project<BaseEntity>(projection).Limit(parameter.Take).ToListAsync();
            
            return result.ToMarkers();

        }

        public async Task<List<SearchItem>> GetNearestAsync(LocationParameter parameter)
        {
            var filter = BuildGeoFilter(parameter);

            List<BaseEntity> result = await _collection.Find(filter).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();

            return result.ToSearchItems();
        }

        public async Task<List<Marker>> GetSpecificMarkersAsync(IReadOnlyCollection<string> logins)
        {
            if (logins == null || logins.Count == 0) return new List<Marker>();

            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);

            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return result.ToMarkers();
        }

        public async Task<List<SearchItem>> GetSpecificAsync(IReadOnlyCollection<string> logins)
        {
            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).ToListAsync();
            return result.ToSearchItems();
        }

        public async Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            //db.bases.find({"user":"rogulenkoko"},{"log":1})
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entities = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            var result = entities.Select(x => x.Login).ToList();
            return result;
        }

        public async Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
        {
            //db.bases.aggregate([
            //{ $match: { user: "rogulenkoko" } },
            //{ $group: { _id: "$t", count: {$sum: 1} } } 
            //])

            var countResult = await _collection.Aggregate()
                .Match(x => x.UserLogin == userLogin)
                .Group(x => x.EntityType, y => new UserEntitiesCountInfo()
                {
                    EntityType = y.Key,
                    Count = y.Count(),
                    Logins = y.Select(x => x.Login).ToList()
                }).ToListAsync();

            return countResult;
        }

        public async Task<byte[]> GetFileAsync(string fileId)
        {
            try
            {
                var fileInfos = await _fileStorage.DownloadAsBytesAsync(fileId);
                return fileInfos;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<Profile>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = BuildFilter(commonFilter, parameter) & BuildGeoFilter(commonFilter);

            var result = await _collection.Find(filter).Skip(commonFilter.Skip).Limit(commonFilter.Take).ToListAsync();
            return result.ToSpecificProfiles();
        }

        public async Task<List<Marker>> GetFilteredMarkersAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = BuildFilter(commonFilter, parameter) & BuildGeoFilter(commonFilter);

            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);

            var result = await _collection.Find(filter).Project<BaseEntity>(projection).Skip(commonFilter.Skip).Limit(commonFilter.Take).ToListAsync();
            return result.ToMarkers();
        }

        public async Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = BuildFilter(commonFilter, parameter) & BuildGeoFilter(commonFilter);
            var profection = Builders<BaseEntity>.Projection.Include(x => x.Login);

            var result = await _collection.Find(filter).Project<BaseEntity>(profection).Limit(commonFilter.Limit).CountAsync();
            return result;
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            if (String.IsNullOrEmpty(login)) return false;

            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entity = await _collection.Find(x => x.Login == login).Project(projection).FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task<List<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            var filter = Builders<BaseEntity>.Filter.AnyEq(x => x.FavoriteFor, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var favorites = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return favorites?.Select(x => x.Login).ToList();
        }

        private FilterDefinition<BaseEntity> BuildFilter(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = Builders<BaseEntity>.Filter.Empty;

            filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);

            if (parameter != null)
            {
                filter = filter & _filterFactory.CreateFilter(parameter);
            }

            if (commonFilter.EntityType != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.EntityType, commonFilter.EntityType);
            }

            if (!String.IsNullOrEmpty(commonFilter.SearchText))
            {
                filter = filter & (Builders<BaseEntity>.Filter.Regex(x => x.Name, $"/{commonFilter.SearchText}/i")
                                  | Builders<BaseEntity>.Filter.Regex(x => x.Login, $"/{commonFilter.SearchText}/i"));
            }

            if (!String.IsNullOrEmpty(commonFilter.UserLogin))
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, commonFilter.UserLogin);
            }

            return filter;
        }

        private FilterDefinition<BaseEntity> BuildGeoFilter(ILocationParameter parameter)
        {
            if (!parameter.Longitude.HasValue || !parameter.Latitude.HasValue) return Builders<BaseEntity>.Filter.Empty;

            if (parameter.RadiusKm == null) parameter.RadiusKm = Int32.MaxValue;

            return Builders<BaseEntity>
                .Filter
                .NearSphere(x => x.Location, parameter.Longitude.Value, parameter.Latitude.Value,
                    parameter.RadiusKm.Value / FunkmapConstants.EarthRadius);
        }
        
    }
}
