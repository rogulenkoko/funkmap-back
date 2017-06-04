using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mime;
using System.Reflection;
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

        public class TestDbContextInitializer : DropCreateDatabaseAlways<FakeMusicianDbContext>
            //DropCreateDatabaseIfModelChanges<FakeMusicianDbContext>
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
                    Styles = Styles.Funk | Styles.HipHop,
                    Instrument = InstrumentType.Brass,
                    VkLink = "https://vk.com/id30724049",
                    YouTubeLink = "https://www.youtube.com/user/Urgantshow"
                };

                
                using (var stream = new MemoryStream())
                {
                    var path = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\Images\\avatar.jpg");
                    Image.FromFile(path).Save(stream,ImageFormat.Jpeg);
                    var avatarBytes = stream.ToArray();
                    m1.AvatarImage = avatarBytes;
                }

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
                    Styles = Styles.Funk | Styles.Rock,
                    Instrument = InstrumentType.Drums,
                    FacebookLink = "https://ru-ru.facebook.com/"
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
                    Styles = Styles.HipHop,
                    Instrument = InstrumentType.Keyboard
                };

                musicianRepository.Add(m1);
                musicianRepository.Add(m2);
                musicianRepository.Add(m3);

                musicianRepository.SaveAsync().Wait();
            }
        }
    }
}
