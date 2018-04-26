using System.Linq;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Messenger.Tests.Queries
{
    [TestClass]
    public class DialogMessagesTest
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
        public void GetDialogMessagesTest()
        {
            var dialog = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult();

            var query = new DialogMessagesQuery
            {
                DialogId = dialog.Id.ToString(),
                UserLogin = dialog.Participants.First(),
                Skip = 0,
                Take = 100
            };
            var queryResult = _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(query).GetAwaiter().GetResult();
            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Messages.Count, 0);
            Assert.IsTrue(queryResult.Messages.Count <= query.Take);
            Assert.IsTrue(queryResult.Messages.All(x => x.DialogId == query.DialogId));
        }
    }
}
