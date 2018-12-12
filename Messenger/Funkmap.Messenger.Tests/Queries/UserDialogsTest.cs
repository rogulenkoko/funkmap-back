using System;
using System.Linq;
using System.Threading;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.EventHandlers;
using Funkmap.Messenger.Contracts.Events.Messages;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Tools;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Funkmap.Messenger.Tests.Queries
{
    public class UserDialogsTest
    {
        private readonly IQueryContext _queryContext;
        private readonly ICommandBus _commandBus;
        private readonly TestToolRepository _testToolRepository;

        public UserDialogsTest()
        {
            var builder = new TestInitializer().Initialize();
            builder.RegisterType<DialogLastMessageEventHandler>()
                .As<IEventHandler<MessageSavedCompleteEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            var container = builder.Build();

            _queryContext = container.Resolve<IQueryContext>();
            _commandBus = container.Resolve<ICommandBus>();

            _testToolRepository = new TestToolRepository(container.Resolve<IMongoCollection<DialogEntity>>());
        }

        [Fact]
        public void GetUserDialogsTest()
        {
            var user = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult().Participants.First();

            var query = new UserDialogsQuery(user);
            var queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.True(queryResult.Success);
            Assert.NotEqual(queryResult.Dialogs.Count, 0);
            Assert.True(queryResult.Dialogs.All(x => x.Participants.Contains(user)));
        }

        [Fact]
        public void UpdateLastMessageTest()
        {
            var dialog = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult();
            var user = dialog.Participants.First();

            var query = new UserDialogsQuery(user);
            var queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.True(queryResult.Success);
            Assert.NotEqual(queryResult.Dialogs.Count, 0);
            Assert.True(queryResult.Dialogs.All(x => x.Participants.Contains(user)));

            var command = new SaveMessageCommand()
            {
                Sender = user,
                DialogId = dialog.Id.ToString(),
                Text = Guid.NewGuid().ToString()
            };

            _commandBus.ExecuteAsync(command).GetAwaiter().GetResult();
            
            Thread.Sleep(300); //awaiting command execution

            queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.True(queryResult.Success);
            Assert.NotEqual(queryResult.Dialogs.Count, 0);
            Assert.True(queryResult.Dialogs.All(x => x.Participants.Contains(user)));

            var lastUpdatedDialog = queryResult.Dialogs.First();
            Assert.Equal(lastUpdatedDialog.DialogId, dialog.Id.ToString());
            Assert.NotNull(lastUpdatedDialog.LastMessage);
            Assert.Equal(lastUpdatedDialog.LastMessage.Text, command.Text);
            Assert.NotEqual(lastUpdatedDialog.LastMessage.Id, ObjectId.Empty.ToString());

            var dialogQuery = new UserDialogQuery(dialog.Id.ToString(), user);
            var dialogQueryResult = _queryContext.ExecuteAsync<UserDialogQuery, UserDialogResponse>(dialogQuery).GetAwaiter().GetResult();

            Assert.True(dialogQueryResult.Success);
            Assert.NotNull(dialogQueryResult.Dialog);
            Assert.True(dialogQueryResult.Dialog.Participants.Contains(user));

            var lastUpdatedDialog2 = dialogQueryResult.Dialog;
            Assert.Equal(lastUpdatedDialog2.DialogId, dialog.Id.ToString());
            Assert.NotNull(lastUpdatedDialog2.LastMessage);
            Assert.Equal(lastUpdatedDialog2.LastMessage.Text, command.Text);
            Assert.NotEqual(lastUpdatedDialog2.LastMessage.Id, ObjectId.Empty.ToString());
        }
    }
}
