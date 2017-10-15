using Autofac;
using Funkmap.Contracts.Notifications;
using Funkmap.Data.Caches;
using Funkmap.Data.Caches.Base;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Services;
using Funkmap.Services.Abstract;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;
using ServiceStack.Redis;

namespace Funkmap.Module
{
    public partial class FunkmapModule 
    {
        private void RegisterDomainDependiences(ContainerBuilder builder)
        {
            builder.RegisterType<FavoriteCacheService>().As<IFavoriteCacheService>();

            var baseRepositoryName = nameof(IBaseRepository);
            builder.RegisterType<BaseRepository>().SingleInstance().Named<IBaseRepository>(baseRepositoryName);
            builder.RegisterDecorator<IBaseRepository>((container, inner) =>
            {
                var redisClient = container.Resolve<IRedisClient>();
                var favoriteService = container.Resolve<IFavoriteCacheService>();
                return new BaseCacheRepository(redisClient, favoriteService , inner);
            }, fromKey: baseRepositoryName).As<IBaseRepository>();


            builder.RegisterType<MusicianRepository>().As<IMusicianRepository>().SingleInstance();
            builder.RegisterType<BandRepository>().As<IBandRepository>().SingleInstance();
            builder.RegisterType<ShopRepository>().As<IShopRepository>().SingleInstance();
            builder.RegisterType<RehearsalPointRepository>().As<IRehearsalPointRepository>();
            builder.RegisterType<StudioRepository>().As<IStudioRepository>();

            builder.RegisterType<FilterFactory>().As<IFilterFactory>();
            
            builder.RegisterType<ParameterFactory>().As<IParameterFactory>();

            builder.RegisterType<EntityUpdateService>().As<IEntityUpdateService>();
            

            builder.RegisterType<BandFilterService>().As<IFilterService>();
            builder.RegisterType<MusicianFilterService>().As<IFilterService>();

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
            
            builder.RegisterType<BandUpdateService>().As<IBandUpdateService>();
            builder.RegisterType<BandUpdateService>().As<IDependenciesController>();

            builder.RegisterType<InviteToGroupNotifications>().As<INotificationTypes>();
        }
    }
}
