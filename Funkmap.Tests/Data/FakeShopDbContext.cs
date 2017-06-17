using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Shop.Data;
using Funkmap.Shop.Data.Entities;
using Funkmap.Shop.Data.Repositories;

namespace Funkmap.Tests.Data
{
    public class FakeShopDbContext:ShopContext
    {
        public FakeShopDbContext() : base("TestConnection")
        {
            Database.SetInitializer(new ShopTestDbContextInitializer());
        }

        public class ShopTestDbContextInitializer : DropCreateDatabaseIfModelChanges<FakeShopDbContext>
// DropCreateDatabaseAlways<FakeShopDbContext>
        {
            protected override void Seed(FakeShopDbContext context)
            {
                SeedShops(context);
                context.SaveChanges();
            }

            private void SeedShops(FakeShopDbContext context)
            {
                var shopRepository = new ShopRepository(context);

                var s1 = new ShopEntity()
                {
                    StoreName = "Гитарушки",
                    Longitude = 32,
                    Latitude = 52,
                    URLShop = "https://ru.wikipedia.org/wiki/C_Sharp"
                };

                var s2 = new ShopEntity()
                {
                    StoreName = "Пинк и Понк",
                    Longitude = 33,
                    Latitude = 51,
                    URLShop = "http://online-simpsons.ru"
                };

                var s3 = new ShopEntity()
                {
                    StoreName = "искать",
                    Longitude = 31,
                    Latitude = 54,
                    URLShop = "https://сайт.com"
                };
                var s4 = new ShopEntity()
                {
                    StoreName = "один",
                    Longitude = 31,
                    Latitude = 51,
                    URLShop = "http://tttt.ru"
                };
                shopRepository.Add(s1);
                shopRepository.Add(s2);
                shopRepository.Add(s3);
                shopRepository.Add(s4);
            }
        }
    }
}
