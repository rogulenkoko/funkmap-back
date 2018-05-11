using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Contracts;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Messenger.Tests.Queries
{
    [TestClass]
    public class DialogsNewMessagesCountTest
    {
        private IQueryContext _queryContext;

        private TestToolRepository _testToolRepository;

        [TestInitialize]
        public void Initialize()
        {
            var container = new TestInitializer().Initialize().Build();

            _queryContext = container.Resolve<IQueryContext>();

            _testToolRepository = new TestToolRepository(container.Resolve<IMongoCollection<DialogEntity>>());
        }


        [TestMethod]
        public void GetDialogsNewMessagesCount()
        {
            var dialog = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult();
            var user = dialog.Participants.First();
            var query = new DialogsNewMessagesCountQuery(user);

            var result = _queryContext.ExecuteAsync<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>(query).GetAwaiter().GetResult();
            Assert.IsTrue(result.Success);
            Assert.AreNotEqual(result.CountResults.Count, 0);


            var userDialogsQuery = new UserDialogsQuery(user);
            var userDialogsQueryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(userDialogsQuery).GetAwaiter().GetResult();

            var allMessages = new List<Message>();

            foreach (var userDialog in userDialogsQueryResult.Dialogs)
            {
                var dialogMessagesQuery = new DialogMessagesQuery
                {
                    DialogId = userDialog.DialogId,
                    UserLogin = user,
                    Skip = 0,
                    Take = Int32.MaxValue
                };
                var dialogMessagesQueryResult = _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(dialogMessagesQuery)
                    .GetAwaiter().GetResult();
                allMessages.AddRange(dialogMessagesQueryResult.Messages);

            }

            Assert.AreEqual(allMessages.Where(x => x.IsNew).GroupBy(x => x.DialogId).Count(), result.CountResults.Count);
        }
    }
}
