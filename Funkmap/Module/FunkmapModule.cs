using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tools;
using MongoDB.Driver;

namespace Funkmap
{
    public partial class FunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            RegisterDomainDependiences(builder);
            RegisterMongoDependiences(builder);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен основной модуль");
        }
    }
}
