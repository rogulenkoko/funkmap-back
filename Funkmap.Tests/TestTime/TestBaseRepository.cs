﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Services;
using Funkmap.Tools.Abstract;
using MongoDB.Bson;
using ServiceStack;

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

       

        public async Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            Start(() =>
            {
                var result = _repository.GetAllAsyns().Result;
                return "get " + result.Count + " Entities";
            }, "getAll");
            return null;
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
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

                var result = _repository.GetNearestAsync(parameter).Result;
                return "get " + result.Count + " Entities";
            }, "getnearest");
            return null;
        }

        public async Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter)
        {
           
            if (parameter == null)
            {
                parameter = new FullLocationParameter()
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

        public async Task<ICollection<BaseEntity>> GetSpecificAsync(string[] logins)
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
                var result = _repository.GetSpecificAsync(logins).GetAwaiter().GetResult();
                return $"all Entities {logins.Length} \n get {result.Count} Entities";

            }, "GetSpecific");
            return null;
        }

        public async Task<ICollection<string>> GetUserEntitiesLogins(string userLogin)
        {
           
            if (userLogin == null)
                userLogin = "243VBlokhin148";
           Start(() =>
            {
                var result = _repository.GetUserEntitiesLogins(userLogin).GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetUserEntitiesLogins");
            return null;
        }

        public async Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
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

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
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
                    Instruments = new List<InstrumentType>() { InstrumentType.Bass }
                };
            }
            Start(() =>
            {

                var result = _repository.GetAllFilteredLoginsAsync(commonFilter, parameter).
                    GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetAllFilteredLogins");
            return null;
        }

        public async Task<bool> CheckIfLoginExistAsync(string login)
        {
            if (login == null)
                login = "243YBlokhin159";
            Start(() =>
            {

                var result = _repository.CheckIfLoginExistAsync(login).GetAwaiter().GetResult();
                return $"get {result}";
            }, "CheckIfLoginExist");
            return true;
        }

        public async Task UpdateAsync(BaseEntity entity)
        {
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

        public async Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfo(string userLogin)
        {
            if (userLogin == null)
            {
                userLogin = "243VBlokhin153";
            }
            Start(() =>
            {
                var result = _repository.GetUserEntitiesCountInfo(userLogin).GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetUserEntitiesCountInfo");
            return null;
        }
    }
}