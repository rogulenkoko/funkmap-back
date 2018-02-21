﻿using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Data.Caches.Base;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Services;
using Funkmap.Services.Abstract;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Module
{
    public partial class FunkmapModule 
    {
        private void RegisterDomainDependiences(ContainerBuilder builder)
        {
            builder.RegisterType<FavoriteCacheService>().As<IFavoriteCacheService>();
            builder.RegisterType<FilteredCacheService>().As<IFilteredCacheService>();

            var baseRepositoryName = nameof(IBaseRepository);
            builder.RegisterType<BaseRepository>().SingleInstance().Named<IBaseRepository>(baseRepositoryName).WithAttributeFiltering();
            builder.RegisterDecorator<IBaseRepository>((container, inner) =>
            { 
                 var favoriteService = container.Resolve<IFavoriteCacheService>();
                return new BaseCacheRepository(favoriteService, inner);
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

            builder.RegisterType<FunkmapNotificationService>()
                .As<IFunkmapNotificationService>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();
            
            builder.RegisterType<BandUpdateService>().As<IBandUpdateService>();
            builder.RegisterType<BandUpdateService>().As<IDependenciesController>();
            
        }
    }
}
