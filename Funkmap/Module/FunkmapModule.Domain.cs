using Autofac;
using Funkmap.Contracts.Notifications;
using Funkmap.Data.Caches;
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
            builder.RegisterType<BaseRepository>().As<IBaseRepository>().SingleInstance().Named<IBaseRepository>(nameof(IBaseRepository));
            builder.RegisterDecorator<IBaseRepository>((container, inner) =>
            {
                var redisClient = container.Resolve<IRedisClient>();
                return new BaseCacheRepository(redisClient, inner);
            }, nameof(IBaseRepository));


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

            builder.RegisterType<InviteToGroupNotifications>().As<INotificationTypes>();
        }
    }
}
