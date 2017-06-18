using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : MongoRepository<DialogEntity>, IDialogRepository
    {
        public DialogRepository(IMongoCollection<DialogEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(DialogEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<DialogEntity>> GetUserDialogs(string login)
        {
            return await _collection.Find(dialog => dialog.Participants.Any(participant => participant == login)).ToListAsync();
        }
    }
}
