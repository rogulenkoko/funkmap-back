using System.Linq;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Tools;
using MongoDB.Driver;
using Xunit;

namespace Funkmap.Messenger.Tests.Queries
{
    public class DialogMessagesTest
    {
        private readonly IQueryContext _queryContext;
        private readonly TestToolRepository _testToolRepository;

        public DialogMessagesTest()
        {
            var container = new TestInitializer().Initialize().Build();

            _queryContext = container.Resolve<IQueryContext>();

            _testToolRepository = new TestToolRepository(container.Resolve<IMongoCollection<DialogEntity>>());
        }

        [Fact]
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
            Assert.True(queryResult.Success);
            Assert.NotEqual(queryResult.Messages.Count, 0);
            Assert.True(queryResult.Messages.Count <= query.Take);
            Assert.True(queryResult.Messages.All(x => x.DialogId == query.DialogId));
        }
    }
}
