using System;
using System.Linq;
using System.Threading;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.EventHandlers;
using Funkmap.Messenger.Contracts.Events.Messages;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Tests.Queries
{
    [TestClass]
    public class UserDialogsTest
    {
        private IQueryContext _queryContext;
        private ICommandBus _commandBus;

        private TestToolRepository _testToolRepository;

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void GetUserDialogsTest()
        {
            var user = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult().Participants.First();

            var query = new UserDialogsQuery(user);
            var queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Dialogs.Count, 0);
            Assert.IsTrue(queryResult.Dialogs.All(x => x.Participants.Contains(user)));
        }

        [TestMethod]
        public void UpdateLastMessageTest()
        {
            var dialog = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult();
            var user = dialog.Participants.First();

            var query = new UserDialogsQuery(user);
            var queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Dialogs.Count, 0);
            Assert.IsTrue(queryResult.Dialogs.All(x => x.Participants.Contains(user)));

            var command = new SaveMessageCommand()
            {
                Sender = user,
                DialogId = dialog.Id.ToString(),
                Text = Guid.NewGuid().ToString()
            };

            _commandBus.ExecuteAsync(command).GetAwaiter().GetResult();
            
            Thread.Sleep(300); //awaiting command execution

            queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Dialogs.Count, 0);
            Assert.IsTrue(queryResult.Dialogs.All(x => x.Participants.Contains(user)));

            var lastUpdatedDialog = queryResult.Dialogs.First();
            Assert.AreEqual(lastUpdatedDialog.DialogId, dialog.Id.ToString());
            Assert.IsNotNull(lastUpdatedDialog.LastMessage);
            Assert.AreEqual(lastUpdatedDialog.LastMessage.Text, command.Text);
            Assert.AreNotEqual(lastUpdatedDialog.LastMessage.Id, ObjectId.Empty);
        }
    }
}
