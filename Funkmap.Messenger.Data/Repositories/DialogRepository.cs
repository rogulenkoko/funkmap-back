using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly IMongoCollection<DialogEntity> _collection;

        public DialogRepository(IMongoCollection<DialogEntity> collection)
        {
            _collection = collection;
        }

        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter)
        {
            var participantsFilter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, parameter.Login);

            var dialogs = await _collection
                .Find(participantsFilter)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();

            return dialogs;
        }

        public async Task<ICollection<string>> GetDialogMembers(string id)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }

        public async Task CreateAsync(DialogEntity item)
        {
            await _collection.InsertOneAsync(item);
        }
    }

}
