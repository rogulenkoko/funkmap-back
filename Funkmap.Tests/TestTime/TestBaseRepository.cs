using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Services;
using Funkmap.Tools.Abstract;
using MongoDB.Bson;
using ServiceStack;

namespace Funkmap.Tests.TestTime
{
    public class TestBaseRepository
    {
        private string part = ".//time//";
        private readonly IBaseRepository _repository;
        

        public TestBaseRepository(IBaseRepository repository)
        {
            _repository = repository;
            DirectoryInfo directory = new DirectoryInfo(".//time");
            if (!directory.Exists)
                directory.Create();
            string[] nameFile =
            {
                "GetUserEntitiesCountInfo",
                "Update",
                "CheckIfLoginExist",
                "GetAllFilteredLogins",
                "GetFiltered",
                "getAll",
                "getnearest",
                "GetFullNearest",
                "GetSpecific",
                "GetUserEntitiesLogins"
            };
            foreach (string name in nameFile)
            {
                FileInfo fileInfo = new FileInfo(part+name+".txt");
                if (!fileInfo.Exists)
                    fileInfo.Create();
            }
        }

        public void TimeGetAll()
        {
            Start(() =>
            {
                var result = _repository.GetAllAsyns().GetAwaiter().GetResult();
                return "get " +result.Count +" Entities";
            },"getAll");  
        }

        public void TimeGetNearest(LocationParameter myLoc = null)
        {
            var loc = new LocationParameter()
            {
                Latitude = 40,
                Longitude = 40,
                RadiusDeg = 10
            };
            if (myLoc != null)
                loc = myLoc;
            Start(() =>
            {
                
                var result = _repository.GetNearestAsync(loc).GetAwaiter().GetResult();
                return "get " + result.Count + " Entities";
            }, "getnearest");
        }

        public void TimeGetFullNearest(FullLocationParameter myLoc = null)
        {
            var loc = new FullLocationParameter()
            {
                Latitude = 40,
                Longitude = 40,
                RadiusDeg = 10,
                Skip = 2,
                Take = 1
            };
            if (myLoc != null)
                loc = myLoc;
            Start(() =>
            {
                
                var result = _repository.GetFullNearestAsync(loc).GetAwaiter().GetResult();
                return "get " + result.Count + " Entities";
            }, "GetFullNearest");
        }

        public void TimeGetSpecific(string[] myLogins = null)
        {
            string[] logins = {"0ASmirnov35",
                   "17AVinogradov34",
                   "17AVinogradov61",
                   "243VBlokhin146",
                   "243PBlokhin124",
                   "243BBlokhin74",
                   "monkey178" ,
                   "IStudio141" };
                if (myLogins != null)
                    logins = myLogins;
            Start(() =>
            {   
                var result = _repository.GetSpecificAsync(logins).GetAwaiter().GetResult();
                return $"all Entities {logins.Length} \n get {result.Count} Entities";

            }, "GetSpecific");
        }

        public void TimeGetUserEntitiesLogins(string myUser = null)
        {
            string user = "243VBlokhin148";
                if (myUser != null)
                    user = myUser;
            Start(() =>
            {
               var result = _repository.GetUserEntitiesLogins(user).GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetUserEntitiesLogins");
        }

        public void TimeGetFiltered(CommonFilterParameter commonFilter = null,
                                        IFilterParameter parameter = null)
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
                    parameter=new MusicianFilterParameter()
                    {
                        Styles = new List<Styles>() { Styles.HipHop},
                        Instruments = new List<InstrumentType>() { InstrumentType.Bass}
                    };
                }
            Start(() =>
            {
                var result = _repository.GetFilteredAsync(commonFilter,parameter).
                GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetFiltered");
        }

        public void TimeGetAllFilteredLogins(CommonFilterParameter commonFilter=null,
                                                    IFilterParameter parameter=null)
        {
                if (commonFilter == null)
                {
                    commonFilter=new CommonFilterParameter()
                    {
                        EntityType = EntityType.Musician
                    };
                }
                if (parameter == null)
                {
                    parameter=new MusicianFilterParameter()
                    {
                        Styles = new List<Styles>() { Styles.HipHop },
                        Instruments = new List<InstrumentType>() { InstrumentType.Bass }
                    };
                }
            Start(() =>
            {
                
                var result = _repository.GetAllFilteredLoginsAsync(commonFilter,parameter).
                GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetAllFilteredLogins");
        }

        public void TimeCheckIfLoginExist(string login = null)
        {
            if (login == null)
                   login = "243YBlokhin159";
            Start(() =>
            {
               
                var result = _repository.CheckIfLoginExistAsync(login).GetAwaiter().GetResult();
                return $"get {result}";
            }, "CheckIfLoginExist");
        }

        public void TimeUpdate(BaseEntity baseEntity = null)
        {
            if (baseEntity == null)
            {
                baseEntity = new BaseEntity()
                {
                        Id = new ObjectId("59d5597d6d6fd92e10b2526e"),
                        EntityType = EntityType.Shop,
                        Name = "qweqeedghbfvbngfcvb"
                };
            }
            Start(() =>
            {
                
                _repository.UpdateAsync(baseEntity).GetAwaiter().GetResult();
                return "";
            }, "Update");
        }

        public void TimeGetUserEntitiesCountInfo(string log = null)
        {
            if (log == null)
            {
                log = "243VBlokhin153";
            }
            Start(() =>
            {
                var result = _repository.GetUserEntitiesCountInfo(log).GetAwaiter().GetResult();
                return $"get {result.Count}";
            }, "GetUserEntitiesCountInfo");
        }


        private void Start<K>(Func<K> action, string nameText)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = action.Invoke();
            stopwatch.Stop();
            CreatingAndWritingText(nameText, stopwatch, $"{result}");
        }

        private void CreatingAndWritingText(string nameText, Stopwatch stopwatch, string description)
        {
            DateTime time = DateTime.Now;
            FileInfo outFile = new FileInfo(part + nameText + ".txt");
            if (!outFile.Exists)
                outFile.Create();
               
            

            string text =
                "**************** \n" +
                time + "\n" +
                "in Milliseconds \n"+
                "time = " + stopwatch.Elapsed.Milliseconds + "\n" +
                +(stopwatch.Elapsed.Milliseconds-GetMinTime(outFile)) +"\n"+
                description + "\n";
            using (StreamWriter sw = new StreamWriter(outFile.FullName, true))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        private int GetMinTime(FileInfo file)
        {
            int minTime = new int();
            minTime = 1000;
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                while (sr.Peek()!=-1)
                {
                    string text = sr.ReadLine();
                    if (text.Length>8 && text.Substring(0, 7).Equals("time = "))
                    {
                        int time = text.Substring(7, text.Length - 7).ToInt();
                        if (time < minTime)
                            minTime = time;
                    }
                }
            }
            return minTime;
        }
    }
}
