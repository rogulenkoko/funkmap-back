﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Services;
using Funkmap.Tools.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack;
using FileInfo = Funkmap.Data.Objects.FileInfo;

namespace Funkmap.Tests.TestTime
{
    public class TestBaseRepository: IBaseRepository
    {
        
        private readonly IBaseRepository _repository;
        private FileManager _fileManager;
        

        public TestBaseRepository(IBaseRepository repository)
        {
            _repository = repository;
            _fileManager = new FileManager();
            
        }
        private void Start<K>(Func<K> action, string nameText)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = action.Invoke();
            stopwatch.Stop();
            _fileManager.CreatingAndWritingText(nameText, stopwatch, $"{result}");
        }

       

        public Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            Start(() =>
            {
                var result = _repository.GetAllAsyns().GetAwaiter().GetResult();
                return "get " + result.Count + " Entities";
            }, "getAll");
            return null;
        }

        
        public Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            
            if (parameter == null)
            {
                parameter = new LocationParameter()
                {
                    Latitude = 40,
                    Longitude = 40,
                     RadiusDeg = 10
                };
            }
                
            Start(() =>
            {

                var result = _repository.GetNearestAsync(parameter).GetAwaiter().GetResult();
                return "get " + result.Count + " Entities";
            }, "getnearest");
            return null;
        }

        public Task<ICollection<BaseEntity>> GetFullNearestAsync(LocationParameter parameter)
        {
           
            if (parameter == null)
            {
                parameter = new LocationParameter()
                {
                    Latitude = 40,
                    Longitude = 40,
                    RadiusDeg = 10,
                    Skip = 2,
                    Take = 1
                };
            }
            Start(() =>
            {

                var result = _repository.GetFullNearestAsync(parameter).GetAwaiter().GetResult();
                return "get " + result.Count + " Entities";
            }, "GetFullNearest");
            return null;
        }

        

        public Task<ICollection<BaseEntity>> GetSpecificNavigationAsync(string[] logins)
        {
            
            if (logins == null)
            {
                 logins = new string[]
                 {
                    "0ASmirnov35",
                    "17AVinogradov34",
                    "17AVinogradov61",
                    "243VBlokhin146",
                    "243PBlokhin124",
                    "243BBlokhin74",
                    "monkey178",
                    "IStudio141"
                };
            }
            Start(() =>
            {
                var result = _repository.GetSpecificNavigationAsync(logins).GetAwaiter().GetResult();
                return $"all Entities {logins.Length} \n get {result.Count} Entities";

            }, "GetSpecific");
            return null;
        }

        public Task<ICollection<BaseEntity>> GetSpecificFullAsync(string[] logins)
        {
            if (logins == null)
            {
                logins = new string[]
                {
                    "0ASmirnov35",
                    "17AVinogradov34",
                    "17AVinogradov61",
                    "243VBlokhin146",
                    "243PBlokhin124",
                    "243BBlokhin74",
                    "monkey178",
                    "IStudio141"
                };
            }
            Start(() =>
            {
                var result = _repository.GetSpecificFullAsync(logins).GetAwaiter().GetResult();
                return $"all Entities {logins.Length} \n get {result.Count} Entities";

            }, "GetSpecificFullAsync");
            return null;
        }

        public Task<ICollection<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            if (commonFilter == null)
            {
                commonFilter = new CommonFilterParameter()
                {
                    EntityType = EntityType.Musician,
                };
            }
            if (parameter == null)
            {
                parameter = new MusicianFilterParameter()
                {
                    Styles = new List<Styles>() { Styles.HipHop },
                    Instruments = new List<InstrumentType>() { InstrumentType.Bass }
                };
            }
            Start(() =>
            {
                var result = _repository.GetFilteredAsync(commonFilter, parameter).
                    GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetFiltered");
            return null;
        }

        public Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            if (commonFilter == null)
            {
                commonFilter = new CommonFilterParameter()
                {
                    EntityType = EntityType.Musician

                };
            }
            if (parameter == null)
            {
                parameter = new MusicianFilterParameter()
                {
                   Styles = new List<Styles>() { Styles.HipHop },
                    //Instruments = new List<InstrumentType>() { InstrumentType.Bass }
                };
            }
            Start(() =>
            {

                var result = _repository.GetAllFilteredLoginsAsync(commonFilter, parameter).
                    GetAwaiter().GetResult();
                if (result == null)
                    return "get null";
                return $"get {result.Count}";
            }, "GetAllFilteredLogins");
            return null;
        }

        public async Task<bool> CheckIfLoginExistAsync(string login)
        {
            await Task.Yield();
            if (login == null)
                login = "243YBlokhin159";
            Start(() =>
            {

                var result = _repository.CheckIfLoginExistAsync(login).GetAwaiter().GetResult();
                return $"get {result}";
            }, "CheckIfLoginExist");
            return true;
        }

        public Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<FileInfo>> GetFilesAsync(string[] fileIds)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<BaseEntity>> GetAllAsync()
        {
            Start(() =>
            {
                var result =_repository.GetAllAsync().GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetAllAsync");
            return null;
        }

        public Task<BaseEntity> GetAsync(string id)
        {
            if (id == null)
                id = "59d5597d6d6fd92e10b2526e";
            Start(() =>
            {
                var result = _repository.GetAsync(id).GetAwaiter().GetResult();
                return $"get {result}";
            }, "GetAsync");
            return null;
        }

        public Task CreateAsync(BaseEntity item)
        {
            if (item == null)
            {
                item = new BaseEntity()
                {
                    Address = "sdwqeqwe",
                    Description = "vbnm,.gfdsfdg",
                    Login = "sdfdghtfh",
                    Name = "dghfjgkkt",
                    Id = new ObjectId()

                };
            }
            Start(() =>
            {
                _repository.CreateAsync(item).GetAwaiter().GetResult();
                return "goood";
            }, "CreateAsync");
            return null;
        }

        Task<BaseEntity> IMongoRepository<BaseEntity>.DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteResult> DeleteAsync(string id)
        {
            if (id == null)
                id = "123456789456asasas";
            Start(() =>
            {
                _repository.DeleteAsync(id).GetAwaiter().GetResult();
                return true;
            }, "DeleteAsync");
            return null;
        }

        public async Task UpdateAsync(BaseEntity entity)
        {
            await Task.Yield();
            if (entity == null)
            {
                entity = new BaseEntity()
                {
                    Id = new ObjectId("59d5597d6d6fd92e10b2526e"),
                    EntityType = EntityType.Shop,
                    Name = "qweqeedghbfvbngfcvb"
                };
            }
            Start(() =>
            {

                _repository.UpdateAsync(entity).GetAwaiter().GetResult();
                return "";
            }, "Update");
        }
    }
}
