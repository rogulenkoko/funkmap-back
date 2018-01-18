using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Feedback.Command.CommandHandler;
using Funkmap.Feedback.Command.Commands;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Funkmap.Feedback.Command
{
    public class FeedbackCommandModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<FeedbackCommandHandler>().As<ICommandHandler<FeedbackCommand>>().WithAttributeFiltering();

            builder.Register(container =>
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                return new AzureFileStorage(blobClient, FeedbackCollectionNameProvider.FeedbackStorage);
            }).Keyed<AzureFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage).SingleInstance();

            builder.Register(context => context.ResolveKeyed<AzureFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage))
                .Keyed<IFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage)
                .SingleInstance();

        }
    }
}
