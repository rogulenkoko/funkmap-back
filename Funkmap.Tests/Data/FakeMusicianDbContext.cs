using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Module.Musician.Data;

namespace Funkmap.Tests.Data
{
    public class FakeMusicianDbContext : MusicianContext
    {
        public FakeMusicianDbContext() : base("TestConnection")
        {
            Database.SetInitializer(new TestDbContextInitializer());
        }

        public class TestDbContextInitializer : CreateDatabaseIfNotExists<FakeMusicianDbContext>
            //DropCreateDatabaseAlways<TestDbContext>
        {
            protected override void Seed(FakeMusicianDbContext context)
            {
                var musicianRepository = new MusicianRepository(context);

                var m1 = new MusicianEntity()
                {
                    Sex = Sex.Male,
                    Login = "rogulenkoko",
                    BirthDate = DateTime.Now,
                    Expirience = 3,
                    Description = "Описание",
                    Name = "Кирилл Рогуленко"
                    
                };

                musicianRepository.Add(m1);

                musicianRepository.Save();
            }
        }
    }
}
