using System;
using System.Data.Entity;
using Funkmap.Musician.Data;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Tests.Data
{
    public class FakeMusicianDbContext : MusicianContext
    {
        public FakeMusicianDbContext() : base("TestConnection")
        {
            Database.SetInitializer(new TestDbContextInitializer());
        }

        public class TestDbContextInitializer : //CreateDatabaseIfNotExists<FakeMusicianDbContext>
            DropCreateDatabaseAlways<FakeMusicianDbContext>
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
                    Name = "Кирилл Рогуленко",
                    Latitude = 50,
                    Longitude = 30,
                    Styles = Styles.Funk | Styles.HipHop
                };

                var m2 = new MusicianEntity()
                {
                    Sex = Sex.Female,
                    Login = "madlib",
                    BirthDate = DateTime.Now,
                    Expirience = 1,
                    Description = "Большое описание музыканта, тудым сюдым. Как дела братва?",
                    Name = "Madlib",
                    Latitude = 51,
                    Longitude = 30,
                    Styles = Styles.Funk | Styles.Rock
                };

                var m3 = new MusicianEntity()
                {
                    Sex = Sex.Male,
                    Login = "razrab",
                    BirthDate = DateTime.Now,
                    Expirience = 0,
                    Description = "Razrab описание!!!",
                    Name = "Razrab Razrab",
                    Latitude = 51,
                    Longitude = 31,
                    Styles = Styles.HipHop
                };

                musicianRepository.Add(m1);
                musicianRepository.Add(m2);
                musicianRepository.Add(m3);

                musicianRepository.SaveAsync().Wait();
            }
        }
    }
}
