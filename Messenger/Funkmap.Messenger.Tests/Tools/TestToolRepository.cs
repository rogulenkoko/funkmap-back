using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger.Entities;
using MongoDB.Driver;

namespace Funkmap.Messenger.Tests.Tools
{
    public class TestToolRepository
    {
        private readonly IMongoCollection<DialogEntity> _dialogsCollection;

        public TestToolRepository(IMongoCollection<DialogEntity> dialogsCollection)
        {
            _dialogsCollection = dialogsCollection;
        }

        public async Task<DialogEntity> GetAnyDialogAsync()
        {
            var dialogs = await _dialogsCollection.Find(x => true).ToListAsync();
            var random = new Random();
            return dialogs.ElementAt(random.Next(0, dialogs.Count));
        }
    }
}
