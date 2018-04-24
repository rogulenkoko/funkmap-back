using System;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Feedback.Command;
using Funkmap.Feedback.Entities;
using Funkmap.Feedback.EventHandlers;
using Funkmap.Feedback.Events;
using MongoDB.Driver;

namespace Funkmap.Feedback
{
    public class FeedbackModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapFeedbackMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapFeedbackDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "feedback";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<FeedbackEntity>(FeedbackCollectionNameProvider.FeedbackCollectionName))
                .As<IMongoCollection<FeedbackEntity>>();

            builder.RegisterType<FeedbackSavedEventHandler>()
                .As<IEventHandler<FeedbackSavedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate()
                .SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Console.WriteLine("Feedback module has been loaded.");
        }
    }
}
