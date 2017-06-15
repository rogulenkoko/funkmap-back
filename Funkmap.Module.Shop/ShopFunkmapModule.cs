using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Shop.Services;
using Funkmap.Shop.Data;
using Funkmap.Shop.Data.Abstract;
using Funkmap.Shop.Data.Repositories;

namespace Funkmap.Module.Shop
{
    class ShopFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ShopRepository>().As<IShopRepository>();
            builder.RegisterType<ShopContext>().InstancePerRequest();
            builder.RegisterType<ShopSearchService>().As<ISearchService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль магазинов");
        }
    }
}
