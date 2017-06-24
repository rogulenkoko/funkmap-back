using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Messenger
{
    public class MessengerModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessegesCollectionName))
                //.OnActivating(async collection => await collection.Instance.Indexes
                //todo .CreateManyAsync(new List<CreateIndexModel<MessageEntity>>() { }))
                .As<IMongoCollection<MessageEntity>>();

            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                //.OnActivating(async collection => await collection.Instance.Indexes
                //todo .CreateManyAsync(new List<CreateIndexModel<MessageEntity>>() { }))
                .As<IMongoCollection<DialogEntity>>();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<DialogRepository>().As<IDialogRepository>();
        }
    }
}
